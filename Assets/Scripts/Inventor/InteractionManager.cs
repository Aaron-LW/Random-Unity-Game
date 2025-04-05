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
    }

    public Camera Camera;

    public float InteractionDistance;

    public LayerMask InteractionLayer;

    public void Interact(Tool tool)
    {
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit hit, InteractionDistance, InteractionLayer))
        {
            if (hit.collider.CompareTag("Geysir"))
            {
                Geysir GeysirScript = hit.collider.GetComponent<Geysir>();

                Instantiate(hitParticle, hit.point, Quaternion.identity);
                InventoryManager.Instance.AddItem(InventoryManager.Instance.GetItemIDbyName(GeysirScript.spice.Name), Mathf.RoundToInt(GeysirScript.spice.returnValue * GeysirScript.returnMultiplier), 0);
            }
        }
    }
}
