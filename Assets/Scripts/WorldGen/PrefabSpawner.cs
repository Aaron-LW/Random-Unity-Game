using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject Ground;

    float GroundSizeX;
    float GroundSizeZ;

    Collider GroundCollider;

    [Header("Prefabs")]
    public List<PrefabSpawn> prefabSpawns;

    void Start()
    {
        GroundCollider = Ground.GetComponent<Collider>();

        GroundSizeX = GroundCollider.bounds.size.x;
        GroundSizeZ = GroundCollider.bounds.size.z;

        foreach (PrefabSpawn prefabSpawn in prefabSpawns)
        {
            SpawnPrefabs(prefabSpawn.gameObject, prefabSpawn.Chance);
        }
    }

    void SpawnPrefabs(GameObject prefab, float chance)
    {
        for (float x = -(GroundSizeX / 2); x < GroundSizeX / 2; x++)
        {
            for (float z = -(GroundSizeZ / 2); z < GroundSizeZ / 2; z++)
            {
                float zufall = Random.Range(0, 100000);

                if (zufall <= chance)
                {
                    Instantiate(prefab, new Vector3(x, 0, z), Quaternion.AngleAxis(Random.Range(0, 360), transform.up));
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

    public PrefabSpawn(GameObject gameobject, float chance)
    {
        gameObject = gameobject;
        Chance = chance;
    }
}