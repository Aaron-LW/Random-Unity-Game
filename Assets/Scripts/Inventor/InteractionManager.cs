using System.Collections;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public ParticleSystem hitParticle;

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

    public float InteractionDistance;

    public LayerMask InteractionLayer;

    public GameObject SpiceStatViewer;
    public GeysirStatViewer GeysirStatViewer;

    [HideInInspector] public bool CanMine = true; 

    void Start()
    {
        GeysirStatViewer = SpiceStatViewer.GetComponent<GeysirStatViewer>();
    }

    public void CheckForStuff() 
    {
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit hit, InteractionDistance, InteractionLayer))
        {
            string Tag = hit.collider.tag;
        
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
    
    IEnumerator MiningCooldown(float seconds) 
    {
        CanMine = false;
        yield return new WaitForSeconds(seconds);

        CanMine = true;
    }
}
