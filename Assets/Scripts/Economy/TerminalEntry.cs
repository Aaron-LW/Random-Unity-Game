using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerminalEntry : MonoBehaviour, IPointerClickHandler
{
    public Image SpiceImage;
    public TMP_Text SpiceAmountText;

    public Image MoneyImage;
    public TMP_Text MoneyAmountText;

    public Trade Trade;

    [HideInInspector] public Image SelfImage;

    Color OriginalColor;
    
    private enum color 
    {
        Green,
        Red
    }

    public AudioClip TerminalButtonDeny;
    public AudioClip TerminalButtonAccept;

    void Awake()
    {
        SelfImage = GetComponent<Image>();
        OriginalColor = SelfImage.color;
    }

    public void OnPointerClick(PointerEventData pointerEventData) 
    {
        if (InventoryManager.Instance.FindItemAmount(Trade.Spice.Item.ID, Trade.Spice.Amount, 0)) 
        {
            InventoryManager.Instance.RemoveItem(Trade.Spice.Item.ID, Trade.Spice.Amount, 0);
            
            MoneyManager.Instance.Money += Trade.ReturnValue;
            MoneyManager.Instance.UpdateMoneyText();

            StartCoroutine(Highlight(color.Green));
            AudioManager.Instance.PlaySFX(TerminalButtonAccept);
        }
        else 
        {
            StartCoroutine(Highlight(color.Red));
            AudioManager.Instance.PlaySFX(TerminalButtonDeny);
        }
    }
    
    IEnumerator Highlight(color color)
    {
        if (color == color.Green) { SelfImage.color = Color.green; }
        else { SelfImage.color = Color.red; }    
        
        while (SelfImage.color.a > OriginalColor.a)
        {
            SelfImage.color = new Color(SelfImage.color.r - 2f * Time.deltaTime, SelfImage.color.g - 2f * Time.deltaTime, SelfImage.color.b - 2f * Time.deltaTime, SelfImage.color.a - 2f * Time.deltaTime);
            yield return null;
        }

        SelfImage.color = OriginalColor;
    }
}
