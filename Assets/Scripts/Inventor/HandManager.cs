using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    private GameObject CurrActiveToolModel;
    private Tool CurrActiveTool;

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

    public void SetHandItem(Tool tool)
    {
        if (CurrActiveToolModel != null && tool.Model != CurrActiveToolModel) { CurrActiveToolModel.SetActive(false); }

        tool.Model.SetActive(true);
        CurrActiveToolModel = tool.Model;
        CurrActiveTool = tool;
    }

    public void ResetHandItem()
    {
        if (CurrActiveToolModel != null) { CurrActiveToolModel.SetActive(false); CurrActiveToolModel = null; }
        CurrActiveTool = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CurrActiveTool != null)
        {
            InteractionManager.Instance.Interact(CurrActiveTool);
        }
    }
}
