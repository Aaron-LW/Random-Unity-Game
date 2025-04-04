using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Camera Camera;

    public float InteractionDistance;

    public LayerMask InteractionLayer;

    void Update()
    {
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit hit, InteractionDistance, InteractionLayer))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.CompareTag("Geysir"))
                {
                    Geysir SpiceScript = hit.collider.gameObject.GetComponent<Geysir>();
                    Debug.Log(SpiceScript.spice.Name);
                }
            }
        }
    }
}
