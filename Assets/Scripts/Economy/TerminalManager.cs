using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerminalManager : MonoBehaviour
{
    public static TerminalManager Instance { get; private set; }

    public GameObject TerminalUI;
    public GameObject TerminalEntry;
    public GameObject SellingSection;

    [HideInInspector] public bool TerminalOpen = false; 

    public List<Terminal> Terminals = new List<Terminal>();

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

        TerminalUI.SetActive(false);

        Terminals.Add(new Terminal(Terminals.Count, 5, null));
    }
    
    public void ToggleTerminal() 
    {
        if (TerminalOpen)  
        {
            TerminalOpen = false;
            InteractionManager.Instance.UIOpen = false;
            InteractionManager.Instance.UnlockCamera();
            
            TerminalUI.SetActive(false);
            
            return;
        }
        
        if (!TerminalOpen) 
        {
            TerminalOpen = true;
            InteractionManager.Instance.UIOpen = true;
            InteractionManager.Instance.LockCamera();
            
            TerminalUI.SetActive(true);
            
            TerminalUIManger.Instance.UpdateTerminalUI(Terminals[0]);
            
            return;
        }
    }
}

public class Terminal 
{
    public int ID;
    public int SellSlots;
    public List<Trade> Trades = new List<Trade>();

    public Terminal(int id, int sellslots, List<Trade> trades) 
    {
        ID = id;
        SellSlots = sellslots;
        Trades = trades;
    }
}

public class Trade 
{
    public ItemStack Spice;
    public int ReturnValue;
    
    public Trade(ItemStack spice, int returnValue) 
    {
        Spice = spice;
        ReturnValue = returnValue;
    }
}
