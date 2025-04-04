using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Geysir : MonoBehaviour
{
    public ParticleSystem partikel;

    [HideInInspector] public Spice spice;

    int countDown;

    void Start()
    {
        countDown = Random.Range(1000, 5000);
        spice = SpiceManager.Instance.Spices[Random.Range(0, SpiceManager.Instance.Spices.Count)];

        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 50))
        {
            transform.position = hit.point;
        }
        else
        {
            Destroy(gameObject);
        }

        partikel.GetComponent<Renderer>().material.color = spice.Color;
    }

    void Update()
    {
        countDown--;

        if (countDown == 0)
        {
            partikel.Play();
            countDown = Random.Range(3000, 10000);
        }
    }
}
