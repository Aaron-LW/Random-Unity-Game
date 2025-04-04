using System.Collections.Generic;
using UnityEngine;

public class SpiceManager : MonoBehaviour
{
    public static SpiceManager Instance { get; private set; }

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

    public Spice(string name, Color color)
    {
        Name = name;
        Color = color;
    }
}
