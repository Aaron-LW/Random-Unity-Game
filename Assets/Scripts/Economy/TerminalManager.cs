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

    }
    
    public void OpenTerminal(Terminal Terminal) 
    {
        TerminalOpen = true;
            InteractionManager.Instance.UIOpen = true;
            InteractionManager.Instance.LockCamera();
            
            TerminalUI.SetActive(true);
            
            TerminalUIManger.Instance.UpdateTerminalUI(Terminal);
            
            return;
    }
    
    public void CloseTerminal() 
    {
        if (TerminalOpen)  
        {
            TerminalOpen = false;
            InteractionManager.Instance.UIOpen = false;
            InteractionManager.Instance.UnlockCamera();
            
            TerminalUI.SetActive(false);
            
            return;
        }
    }
}

public class Terminal 
{
    public int SellSlots;
    public List<Trade> Trades = new List<Trade>();

    public Terminal(int sellslots, List<Trade> trades) 
    {
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
