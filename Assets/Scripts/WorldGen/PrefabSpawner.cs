using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject Ground;
    public LayerMask GroundLayer;

    float GroundSizeX;
    float GroundSizeZ;

    Collider GroundCollider;

    public float GroundCheckDistance;

    [Header("Prefabs")]
    public List<PrefabSpawn> prefabSpawns;

    void Start()
    {
        GroundCollider = Ground.GetComponent<Collider>();

        GroundSizeX = GroundCollider.bounds.size.x;
        GroundSizeZ = GroundCollider.bounds.size.z;

        foreach (PrefabSpawn prefabSpawn in prefabSpawns)
        {
            SpawnPrefabs(prefabSpawn);
        }
    }

    void SpawnPrefabs(PrefabSpawn prefab)
    {
        Collider prefabCollider = null;
        if (prefab.AdjustYPos) { prefabCollider = prefab.gameObject.transform.GetComponent<Collider>(); }
    
        for (float x = -(GroundSizeX / 2); x < GroundSizeX / 2; x++)
        {
            for (float z = -(GroundSizeZ / 2); z < GroundSizeZ / 2; z++)
            {
                float zufall = Random.Range(0, 100000);

                if (zufall <= prefab.Chance)
                {
                    GameObject InstantiatedPrefab = Instantiate(prefab.gameObject, new Vector3(x, Ground.transform.position.y + 1, z), Quaternion.Euler(prefab.Orientation));

                    if (Physics.Raycast(InstantiatedPrefab.transform.position, Vector3.down, out RaycastHit hit, GroundCheckDistance, GroundLayer)) 
                    {
                        if (prefab.AdjustYPos && prefabCollider != null) 
                        {
                            InstantiatedPrefab.transform.position = new Vector3(InstantiatedPrefab.transform.position.x, hit.point.y + 1, InstantiatedPrefab.transform.position.z);
                        }
                        else 
                        {
                            InstantiatedPrefab.transform.position = new Vector3(InstantiatedPrefab.transform.position.x, hit.point.y, InstantiatedPrefab.transform.position.z);
                        }
                    }
                    else 
                    {
                        Destroy(InstantiatedPrefab.gameObject);
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class PrefabSpawn
{
    public GameObject gameObject;
    public float Chance;
    public Vector3 Orientation;
    public bool AdjustYPos = true;

    public PrefabSpawn(GameObject gameobject, float chance, Vector3 orientation = new Vector3(), bool adjustypos = true)
    {
        gameObject = gameobject;
        Chance = chance;
        Orientation = orientation;
        AdjustYPos = adjustypos;
    }
}