using System.Collections.Generic;
using UnityEngine;
using System;

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
    
    [HideInInspector] public enum Type 
    {
        Geysir,
        Terminal
    }

    void Start()
    {
        GroundCollider = Ground.GetComponent<MeshCollider>();

        GroundSizeX = GroundCollider.bounds.size.x * 2;
        GroundSizeZ = GroundCollider.bounds.size.z * 2;

        foreach (PrefabSpawn prefabSpawn in prefabSpawns)
        {
            SpawnPrefabs(prefabSpawn);
        }
    }

    void SpawnPrefabs(PrefabSpawn prefab)
    {
        Collider prefabCollider = null;
        if (prefab.AdjustYPos) { prefabCollider = prefab.gameObject.transform.GetComponent<Collider>(); }

        for (float x = 0; x < GroundSizeX / 2; x++)
        {
            for (float z = 0; z < GroundSizeZ / 2; z++)
            {
                float zufall = UnityEngine.Random.Range(0, 100000);

                if (zufall <= prefab.Chance)
                {
                    GameObject InstantiatedPrefab = Instantiate(prefab.gameObject, new Vector3(x, Ground.transform.position.y + GroundCheckDistance - 1, z), Quaternion.Euler(prefab.Orientation));
                    
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
                        
                        if (prefab.AlignToNormal) 
                        {
                            InstantiatedPrefab.transform.rotation = Quaternion.LookRotation(InstantiatedPrefab.transform.forward, hit.normal);
                        }
                        
                        if (prefab.RandomZRotation) 
                        {
                            InstantiatedPrefab.transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)));
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
    public bool AdjustYPos;
    public bool RandomZRotation;
    public bool AlignToNormal;
    public PrefabSpawner.Type Type;

    public PrefabSpawn(GameObject gameobject, PrefabSpawner.Type type, float chance, bool adjustypos, bool randomzrotation, bool aligntonormal, Vector3 orientation = new Vector3())
    {
        gameObject = gameobject;
        Chance = chance;
        Orientation = orientation;
        AdjustYPos = adjustypos;
        RandomZRotation = randomzrotation;
        AlignToNormal = aligntonormal;
        Type = type;
    }
}