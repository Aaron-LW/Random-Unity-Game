using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class RecipePack : MonoBehaviour, IPointerClickHandler
{
    public TMP_Text PackNameText;
    public TMP_Text PackCostText;

    public AudioClip ButtonAccept;
    public AudioClip ButtonDeny;
    
    [HideInInspector] public Image SelfImage;
    Color OriginalColor;
    
    private enum color 
    {
        Green,
        Red
    }
    
    void Awake()
    {
        SelfImage = GetComponent<Image>();
        OriginalColor = SelfImage.color;
    }
    
    public void OnPointerClick(PointerEventData e)
    {
        StartCoroutine(Highlight(color.Green));
        AudioManager.Instance.PlaySFX(ButtonAccept);
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
