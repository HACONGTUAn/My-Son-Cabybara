using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Components")]
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Text text;

    [Header("Settings")]
    [SerializeField] float duration = 0.07f;
    [SerializeField] Ease ease = Ease.OutQuad;
    [SerializeField] Vector2 animationSizeDelta = new(14f, 7f);
    [SerializeField] float animationFontSizeDelta = 1f;

    Vector2 initialSize;
    float initialFontSize;

    void Start()
    {
        initialSize = rectTransform.sizeDelta;
       // initialFontSize = text.fontSize;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        rectTransform.DOSizeDelta(initialSize - animationSizeDelta, duration)
            .SetEase(ease);

        
        float targetFontSize = initialFontSize - animationFontSizeDelta;
     //   text.fontSize = (int)targetFontSize; 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        rectTransform.DOSizeDelta(initialSize, duration)
            .SetEase(ease);

       
       // text.fontSize = (int)initialFontSize;
    }
}
