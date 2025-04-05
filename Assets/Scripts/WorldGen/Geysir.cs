using UnityEngine;

public class Geysir : MonoBehaviour
{
    public ParticleSystem partikel;
    public int random;

    public Spice spice;

    public float returnMultiplier;
    public enum Quality
    {
        Low,
        Medium,
        High
    }
    
    public Quality quality;

    int countDown;

    void Awake()
    {
        random = Random.Range(0, SpiceManager.Instance.returnMultipliers.Length);
    }

    void Start()
    {
        countDown = Random.Range(1000, 5000);
        spice = SpiceManager.Instance.Spices[Random.Range(0, SpiceManager.Instance.Spices.Count)];
        
        returnMultiplier = SpiceManager.Instance.returnMultipliers[random];
        quality = (Quality)random;

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
