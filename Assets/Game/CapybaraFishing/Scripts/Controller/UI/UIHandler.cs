using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIHandler : MonoBehaviour
{
    
    public void ShowPanel(RectTransform panel)
    {        
        panel.localScale = Vector3.zero;
        panel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack); 
    }

    public void HidePanel(RectTransform panel)
    {      
        panel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack); 
    }
}
