using UnityEngine;
using System.Collections.Generic;

public class TerminalObject : MonoBehaviour 
{
    public Terminal Terminal;

    void Start()
    {
        Terminal = new Terminal(2, new List<Trade>() { new Trade(new ItemStack(InventoryManager.Instance.Items[1], 100), 10), new Trade(new ItemStack(InventoryManager.Instance.Items[2], 30), 10) });
    }
}