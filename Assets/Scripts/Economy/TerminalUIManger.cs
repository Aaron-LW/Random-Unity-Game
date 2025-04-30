using System;
using UnityEngine;

public class TerminalUIManger : MonoBehaviour
{
    public static TerminalUIManger Instance { get; private set; }

    public Sprite MoneySprite;

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
        
        for (int i = 0; i < Terminal.Trades.Count; i++) 
        {
            GameObject Entry = Instantiate(TerminalManager.Instance.TerminalEntry, TerminalManager.Instance.SellingSection.transform);
            TerminalEntry terminalEntry = Entry.GetComponent<TerminalEntry>();

            terminalEntry.SpiceImage.sprite = Terminal.Trades[i].Spice.Item.Sprite;
            terminalEntry.SpiceAmountText.text = Terminal.Trades[i].Spice.Amount.ToString();

            terminalEntry.MoneyImage.sprite = MoneySprite;
            terminalEntry.MoneyAmountText.text = Terminal.Trades[i].ReturnValue.ToString();

            terminalEntry.Trade = Terminal.Trades[i];
        }
    } 
}
