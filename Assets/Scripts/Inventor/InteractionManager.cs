using System;
using System.Collections;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public ParticleSystem hitParticle;

    private string CurrLookingAtTag = null;
    private GameObject CurrLookingAtGameObject = null;

    [HideInInspector] public bool UIOpen = false;

    public AudioClip MineSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        SpiceStatViewer.SetActive(false);
        InvokeRepeating("CheckForStuff", 0.1f, 0.1f);
    }

    public Camera Camera;
    public CameraMovement CameraMovement;

    public float InteractionDistance;

    public LayerMask InteractionLayer;
    public LayerMask GroundLayer;

    public GameObject SpiceStatViewer;
    [HideInInspector] public GeysirStatViewer GeysirStatViewer;

    [HideInInspector] public bool CanMine = true; 

    void Start()
    {
        GeysirStatViewer = SpiceStatViewer.GetComponent<GeysirStatViewer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            TerminalObject terminalObject = null;
            if (CurrLookingAtGameObject != null) { CurrLookingAtGameObject.TryGetComponent<TerminalObject>(out terminalObject); }
        
            if (terminalObject != null) 
            {
                Interact(CurrLookingAtTag, terminalObject);
                return;
            }
            
            Interact(CurrLookingAtTag);
        }
    }

    public void CheckForStuff() 
    {
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit hit, InteractionDistance, InteractionLayer))
        {
            string Tag = hit.collider.tag;

            CurrLookingAtTag = Tag;
            CurrLookingAtGameObject = hit.collider.gameObject;
            
            if (Tag == "Geysir" && !SpiceStatViewer.activeSelf)
            {
                Geysir geysir = hit.collider.GetComponent<Geysir>();
            
                GeysirStatViewer.Quality.text = "Quality: " + geysir.quality.ToString();
                GeysirStatViewer.Type.text = "Type: " + geysir.spice.Name;
                
                SpiceStatViewer.SetActive(true);
            }
        }
        else
        {
            if (SpiceStatViewer.activeSelf) { SpiceStatViewer.SetActive(false); }
            CurrLookingAtTag = null;
            CurrLookingAtGameObject = null;
        }
    }

    public void InteractWithMineable(Tool tool)
    {
        if (!CanMine) { Debug.LogError("Tried to mine but was on cooldown"); return; }

        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit hit, InteractionDistance, InteractionLayer))
        {
            if (hit.collider.CompareTag("Geysir"))
            {
                Geysir GeysirScript = hit.collider.GetComponent<Geysir>();

                Instantiate(hitParticle, hit.point, Quaternion.identity);
                InventoryManager.Instance.AddItem(InventoryManager.Instance.GetItemIDbyName(GeysirScript.spice.Name), Mathf.RoundToInt(GeysirScript.spice.returnValue * GeysirScript.returnMultiplier * tool.yieldMultiplier), 0);
                AudioManager.Instance.PlaySFX(MineSound);
                
                StartCoroutine(MiningCooldown(tool.MiningCooldown));
                return;
            }
        }
    }
    
    public void Interact(string Tag, TerminalObject terminalObject = null) 
    {
        if (UIOpen) { CloseAllUIs(Tag); UIOpen = false; }
        if (Tag == null) { return; }

        if (Tag == "Terminal") 
        {
            if (TerminalManager.Instance.TerminalOpen) 
            {
                TerminalManager.Instance.CloseTerminal();
            }
            else 
            {
                TerminalManager.Instance.OpenTerminal(terminalObject.Terminal);
            }
        }
    }
    
    public void CloseAllUIs(string supposed) 
    {
        if (TerminalManager.Instance.TerminalOpen && supposed == null) { TerminalManager.Instance.CloseTerminal(); }
        if (supposed == null) { return; }
    
        if (InventoryManager.Instance.InventoryOpen && supposed != "Inventory") { InventoryManager.Instance.ToggleInventory(); }
        if (TerminalManager.Instance.TerminalOpen && supposed != "Terminal") { TerminalManager.Instance.CloseTerminal(); }
    }
    
    IEnumerator MiningCooldown(float seconds) 
    {
        CanMine = false;
        yield return new WaitForSeconds(seconds);

        CanMine = true;
    }
    
    public void LockCamera() { CameraMovement.enabled = false; Cursor.lockState = CursorLockMode.None;  Cursor.visible = true; }
    public void UnlockCamera() { CameraMovement.enabled = true; Cursor.lockState = CursorLockMode.Locked;  Cursor.visible = false; }
}
