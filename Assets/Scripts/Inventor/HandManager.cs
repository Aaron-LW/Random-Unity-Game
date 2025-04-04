using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    private GameObject CurrActiveToolModel;

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

    public void SetHandItem(Pickaxe tool)
    {
        if (CurrActiveToolModel != null && tool.Model != CurrActiveToolModel) { CurrActiveToolModel.SetActive(false); }

        tool.Model.SetActive(true);
        CurrActiveToolModel = tool.Model;
    }

    public void ResetHandItem()
    {
        if (CurrActiveToolModel != null) { CurrActiveToolModel.SetActive(false); CurrActiveToolModel = null; }
    }
}
