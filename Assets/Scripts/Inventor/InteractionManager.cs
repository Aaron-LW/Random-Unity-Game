using System.Collections;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public ParticleSystem hitParticle;

    private string CurrLookingAtTag = null;
    private GameObject CurrLookingAtGameObject = null;

    [HideInInspector] public bool UIOpen = false;

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

    public CameraMovement Camera;

    public float InteractionDistance;

    public LayerMask InteractionLayer;

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
            Interact(CurrLookingAtTag);
        }
        
        if (Input.GetMouseButtonDown(1)) 
        {
            
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

                StartCoroutine(MiningCooldown(tool.MiningCooldown));
                return;
            }
        }
    }
    
    public void Interact(string Tag) 
    {
        if (UIOpen) { CloseAllUIs(Tag); UIOpen = false; }
        if (Tag == null) { return; }

        if (Tag == "Terminal") 
        {
            TerminalManager.Instance.ToggleTerminal();
            return;
        }
    }
    
    public void CloseAllUIs(string supposed) 
    {
        if (TerminalManager.Instance.TerminalOpen && supposed == null) { TerminalManager.Instance.ToggleTerminal(); }
        if (supposed == null) { return; }
    
        if (InventoryManager.Instance.InventoryOpen && supposed != "Inventory") { InventoryManager.Instance.ToggleInventory(); }
        if (TerminalManager.Instance.TerminalOpen && supposed != "Terminal") { TerminalManager.Instance.ToggleTerminal(); }
    }
    
    IEnumerator MiningCooldown(float seconds) 
    {
        CanMine = false;
        yield return new WaitForSeconds(seconds);

        CanMine = true;
    }
    
    public void LockCamera() { Camera.enabled = false; Cursor.lockState = CursorLockMode.None;  Cursor.visible = true; }
    public void UnlockCamera() { Camera.enabled = true; Cursor.lockState = CursorLockMode.Locked;  Cursor.visible = false; }
}
