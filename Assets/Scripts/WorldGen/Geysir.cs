using UnityEngine;

public class Geysir : MonoBehaviour
{
    public ParticleSystem partikel;
    
    public int random;
    public int SpiceRandom;
    
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
        
        if (Random.Range(0, 4) <= 2) { SpiceRandom = 0; }
        else { SpiceRandom = 1; }
    }

    void Start()
    {
        countDown = Random.Range(1000, 5000);
        
        spice = SpiceManager.Instance.Spices[SpiceRandom];
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
