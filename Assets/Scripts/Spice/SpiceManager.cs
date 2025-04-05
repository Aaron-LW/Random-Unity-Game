using System.Collections.Generic;
using UnityEngine;

public class SpiceManager : MonoBehaviour
{
    public static SpiceManager Instance { get; private set; }
    public float[] returnMultipliers = new float[3] { 0.9f, 1f, 1.2f };

    public List<Spice> Spices = new List<Spice>();

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

[System.Serializable]
public class Spice
{
    public string Name;
    public Color Color;
    public int returnValue;
    
    [HideInInspector] public enum Quality 
    {
        Low,
        Medium,
        High
    }

    public Spice(string name, Color color, int returnvalue)
    {
        Name = name;
        Color = color;
        returnValue = returnvalue;
    }
}
