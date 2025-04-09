using System;
using UnityEngine;

public class TerminalUIManger : MonoBehaviour
{
    public static TerminalUIManger Instance { get; private set; }

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

    public void UpdateTerminalUI(Terminal Terminal) 
    {
        foreach (Transform child in TerminalManager.Instance.SellingSection.gameObject.transform) 
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < 5 - 1; i++) 
        {
            GameObject Entry = Instantiate(TerminalManager.Instance.TerminalEntry, TerminalManager.Instance.SellingSection.transform);
        }
    } 
}
