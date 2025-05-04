using UnityEngine;

public class PickaxeUpgrade : MonoBehaviour
{
    public GameObject PickaxeHead;

    private Renderer PickaxeRenderer;

    public Color[] Colors;

    private int PickaxeLevel = -1;

    void Awake()
    {
        PickaxeRenderer = PickaxeHead.GetComponent<Renderer>();
        UpgradePickaxe();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            UpgradePickaxe();
        }
        if (Input.GetKeyDown(KeyCode.G)) 
        {
            DegradePickaxe();
        }
    }

    public void UpgradePickaxe() 
    {
        if (PickaxeLevel != Colors.Length - 1) { PickaxeLevel += 1; }
        
        if (PickaxeLevel < Colors.Length) 
        {
            PickaxeRenderer.material.color = Colors[PickaxeLevel]; 
        }
    }
    
    public void DegradePickaxe() 
    {
        if (PickaxeLevel != 0) { PickaxeLevel -= 1; }
        
        if (PickaxeLevel < 0) 
        {
            PickaxeRenderer.material.color = Colors[0];
        }
        else 
        {
            PickaxeRenderer.material.color = Colors[PickaxeLevel];
        }
    }
}
