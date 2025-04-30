using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    public int Money = 0;
    public TMP_Text MoneyText;
    
    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
        else 
        {
            Destroy(this);
        }

        UpdateMoneyText();
    }

    public void UpdateMoneyText() 
    {
        MoneyText.text = Money.ToString();
    }
}
