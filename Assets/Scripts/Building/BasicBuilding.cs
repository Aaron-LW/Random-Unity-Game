using UnityEngine;

public class BasicBuilding : MonoBehaviour
{
    public BasicBuilding Instance { get; private set; }

    void Awake()
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
}
