using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    public GameObject ItemSlot;

    private Outline CurrEnabledOutline = null;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

    }

    public void UpdateInventory(int InventoryIndex)
    {
        if (InventoryIndex > InventoryManager.Instance.Inventories.Count - 1) { Debug.LogError("Inventar versucht zu Updaten aber Inventar existiert nicht"); }

        Inventory CurrInventory = InventoryManager.Instance.Inventories[InventoryIndex];

        foreach (Transform child in CurrInventory.InventoryUIObject.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < CurrInventory.Slots; i++)
        {
            ItemStack CurrItemStack = CurrInventory.Items[i];
            Item CurrItem = CurrItemStack.Item;

            GameObject InstantiatedSlot = Instantiate(ItemSlot, CurrInventory.InventoryUIObject.transform);

            ItemSlotScript SlotScript = InstantiatedSlot.GetComponent<ItemSlotScript>();

            if (CurrItem.Sprite == null) { SlotScript.SpriteObject.color = Color.clear; }
            else { SlotScript.SpriteObject.sprite = CurrItem.Sprite; }

            if (CurrItemStack.Amount == 1 || CurrItemStack.Amount == 0) { SlotScript.TextObject.text = ""; }
            else { SlotScript.TextObject.text = CurrItemStack.Amount.ToString(); }

            if (InventoryIndex == 1)
            {
                if (InventoryManager.Instance.CurrHotbarSlot == i)
                {
                    if (CurrItem is Tool tool) { HandManager.Instance.SetHandItem(tool); }
                    else { HandManager.Instance.ResetHandItem(); }

                    if (CurrEnabledOutline != null) { CurrEnabledOutline.enabled = false; }

                    CurrEnabledOutline = InstantiatedSlot.GetComponent<Outline>();
                    CurrEnabledOutline.enabled = true;
                }
            }
        }
    }
}
