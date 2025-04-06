using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public CameraMovement Camera;
    
    [HideInInspector] public enum ToolType 
    {
        Fist,
        Pickaxe,
        Axe
    }

    [HideInInspector] public List<Inventory> Inventories = new List<Inventory>();
    [HideInInspector] public List<Item> Items = new List<Item>();

    public List<InitItem> InitialisingItems = new List<InitItem>();
    public List<InitTool> InitialisingTools = new List<InitTool>();


    [HideInInspector] public bool InitManagers = false;

    public GameObject MainInventory;
    public GameObject HotbarInventory;

    private bool InventoryOpen = false;

    [HideInInspector] public int CurrHotbarSlot = 0;

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

        foreach (InitItem item in InitialisingItems) { InitItem(item.Name, item.MaxStackSize, item.Sprite); }
        foreach (InitTool tool in InitialisingTools) { InitTool(tool.Name, (ToolType)tool.MiningCooldown, tool.MiningCooldown, tool.yieldMultiplier, tool.Sprite, tool.Model, tool.MaxStackSize); }

        InitInventory(21, MainInventory, 1);
        InitInventory(7, HotbarInventory, 0);

        MainInventory.SetActive(false);

        InitManagers = true;

        InventoryUI.Instance.UpdateInventory(0);
        InventoryUI.Instance.UpdateInventory(1);

        if (!HotbarInventory.activeSelf) { HotbarInventory.SetActive(true); }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddItem(GetItemIDbyName("Pickaxe"), 1, 1);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            RemoveItem(GetItemIDbyName("Pickaxe"), 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (InventoryOpen)
            {
                MainInventory.SetActive(false);
                InventoryOpen = false;

                Camera.enabled = true;
            }
            else
            {
                MainInventory.SetActive(true);
                InventoryOpen = true;

                Camera.enabled = false;
            }
        }

        if (Input.mouseScrollDelta.y > 0 && CurrHotbarSlot > 0) { CurrHotbarSlot--; InventoryUI.Instance.UpdateInventory(1); }
        if (Input.mouseScrollDelta.y < 0 && CurrHotbarSlot < Inventories[1].Slots - 1) { CurrHotbarSlot++; InventoryUI.Instance.UpdateInventory(1); }
    }

    public void AddItem(int ID, int Amount, int InventoryIndex, bool Overflow = false)
    {
        Inventory Inventory = Inventories[InventoryIndex];

        for (int i = 0; i < Inventory.Slots; i++)
        {
            ItemStack CurrItemStack = Inventory.Items[i];
            Item CurrItem = CurrItemStack.Item;

            if (CurrItem.ID == 0)
            {
                if (Amount > Items[ID].MaxStackSize)
                {
                    Inventory.Items[i] = new ItemStack(Items[ID], Items[ID].MaxStackSize);
                    Amount -= Items[ID].MaxStackSize;
                }
                else
                {
                    Inventory.Items[i] = new ItemStack(Items[ID], Amount);

                    InventoryUI.Instance.UpdateInventory(InventoryIndex);
                    return;
                }
            }

            if (CurrItem.ID == ID)
            {
                if (CurrItemStack.Amount + Amount <= CurrItem.MaxStackSize)
                {
                    Inventory.Items[i] = new ItemStack(CurrItem, CurrItemStack.Amount + Amount);

                    InventoryUI.Instance.UpdateInventory(InventoryIndex);
                    return;
                }
                else
                {
                    Amount -= CurrItem.MaxStackSize - CurrItemStack.Amount;
                    Inventory.Items[i] = new ItemStack(CurrItem, CurrItem.MaxStackSize);
                }
            }
        }

        if (Amount > 0)
        {
            if (!Overflow && Inventory.OverflowInventoryIndex < Inventories.Count)
            {
                InventoryUI.Instance.UpdateInventory(InventoryIndex);
                AddItem(ID, Amount, Inventory.OverflowInventoryIndex, true);
            }
            else
            {
                InventoryUI.Instance.UpdateInventory(InventoryIndex);
                Debug.LogError("Nicht genug platz um Item hinzuzufügen");
            }
        }
    }

    public void RemoveItem(int ID, int Amount, int InventoryIndex = 0, bool Overflow = false)
    {
        if (!FindItemAmount(ID, Amount, InventoryIndex))
        {
            Debug.LogError("Nicht genug Items zum Entfernen vorhanden");
            return;
        }

        Inventory Inventory = Inventories[InventoryIndex];

        for (int i = 0; i < Inventory.Slots; i++)
        {
            ItemStack CurrItemStack = Inventory.Items[i];
            Item CurrItem = CurrItemStack.Item;

            if (CurrItem.ID == 0) { continue; }

            if (CurrItem.ID == ID)
            {
                if (CurrItemStack.Amount <= Amount)
                {
                    Amount -= CurrItemStack.Amount;
                    Inventory.Items[i] = new ItemStack(Items[0], 0);

                    InventoryUI.Instance.UpdateInventory(InventoryIndex);
                }
                else
                {
                    Inventory.Items[i] = new ItemStack(CurrItemStack.Item, CurrItemStack.Amount - Amount);

                    InventoryUI.Instance.UpdateInventory(InventoryIndex);
                    return;
                }
            }
        }

        if (Amount > 0 && !Overflow && Inventory.OverflowInventoryIndex >= 0 && Inventory.OverflowInventoryIndex < Inventories.Count)
        {
            InventoryUI.Instance.UpdateInventory(InventoryIndex);
            RemoveItem(ID, Amount, Inventory.OverflowInventoryIndex, true);
        }
    }

    public bool FindItemAmount(int ID, int Amount, int InventoryIndex)
    {
        int foundAmount = 0;
        HashSet<int> checkedIndexes = new HashSet<int>();

        while (InventoryIndex != -1 && InventoryIndex < Inventories.Count)
        {
            if (checkedIndexes.Contains(InventoryIndex))
            {
                break;
            }

            checkedIndexes.Add(InventoryIndex);

            Inventory Inventory = Inventories[InventoryIndex];

            foreach (var CurrItemStack in Inventory.Items)
            {
                if (CurrItemStack.Item.ID == ID)
                {
                    foundAmount += CurrItemStack.Amount;
                }

                if (foundAmount >= Amount)
                {
                    return true;
                }
            }

            if (Inventory.OverflowInventoryIndex < 0 || Inventory.OverflowInventoryIndex >= Inventories.Count)
            {
                break;
            }

            InventoryIndex = Inventory.OverflowInventoryIndex;
        }

        return false;
    }

    void InitInventory(int Slots, GameObject InventoryUIObject, int OverflowInventoryIndex = -1)
    {
        Inventories.Add(new Inventory(Inventories.Count, Slots, OverflowInventoryIndex, new List<ItemStack>(), InventoryUIObject));

        for (int i = 0; i < Slots; i++)
        {
            Inventories[Inventories.Count - 1].Items.Add(new ItemStack(Items[0], 0));
        }
    }

    void InitItem(string Name, int MaxStackSize, Sprite Sprite = null)
    {
        Items.Add(new Item(Items.Count, Name, MaxStackSize, Sprite));
    }

    void InitTool(string Name, ToolType Type, float MiningSpeed, float yieldMultiplier, Sprite Sprite = null, GameObject Modell = null, int MaxStackSize = 1)
    {
        Items.Add(new Tool(Items.Count, Name, Type, MiningSpeed, yieldMultiplier, Sprite, Modell, MaxStackSize));
    }

    public int GetItemIDbyName(string Name)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Name == Name)
            {
                return i;
            }
        }

        return -1;
    }
}

public class Item
{
    public int ID;
    public string Name;
    public int MaxStackSize;
    public Sprite Sprite;

    public Item(int id, string name, int maxstacksize, Sprite sprite = null)
    {
        ID = id;
        Name = name;
        MaxStackSize = maxstacksize;
        Sprite = sprite;
    }
}

public class Tool : Item 
{
    public InventoryManager.ToolType Type;
    public float MiningCooldown;
    public float yieldMultiplier;
    public GameObject Model;
    
    public Tool(int id, string name, InventoryManager.ToolType type, float miningcooldown, float yieldmultiplier, Sprite sprite = null, GameObject model = null, int maxstacksize = 1) : base(id, name, maxstacksize, sprite)
    {
        ID = id;
        Name = name;
        MaxStackSize = maxstacksize;
        Type = type;
        MiningCooldown = miningcooldown;
        yieldMultiplier = yieldmultiplier;
        Model = model;
    }
}

public struct ItemStack
{
    public Item Item;
    public int Amount;

    public ItemStack(Item item, int amount)
    {
        Item = item;
        Amount = amount;
    }
}

public class Inventory
{
    public int ID;
    public int Slots;
    public int OverflowInventoryIndex;
    public List<ItemStack> Items = new List<ItemStack>();
    public GameObject InventoryUIObject;

    public Inventory(int id, int slots, int overflowinventoryindex, List<ItemStack> items, GameObject inventoryuiobject)
    {
        ID = id;
        Slots = slots;
        OverflowInventoryIndex = overflowinventoryindex;
        Items = items;
        InventoryUIObject = inventoryuiobject;
    }
}

[System.Serializable]
public class InitItem
{
    public string Name;
    [HideInInspector] public int MaxStackSize;
    public Sprite Sprite;

    public InitItem(int id, string name, int maxstacksize, Sprite sprite = null)
    {
        Name = name;
        MaxStackSize = maxstacksize;
        Sprite = sprite;
    }
}

[System.Serializable]
public class InitTool : InitItem
{
    public InventoryManager.ToolType Type;
    public float MiningCooldown;
    public float yieldMultiplier;
    public GameObject Model;
    
    public InitTool(int id, string name, InventoryManager.ToolType type, float miningcooldown, float yieldmultiplier, Sprite sprite = null, GameObject model = null, [HideInInspector] int maxstacksize = 1) : base(id, name, maxstacksize, sprite)
    {
        Type = type;
        MiningCooldown = miningcooldown;
        yieldMultiplier = yieldmultiplier;
        Model = model;
    }
}