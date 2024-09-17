using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class UIComboAnimation : MonoBehaviour
{
    [SerializeField] Text comboText;
    [SerializeField] Text encourageText;
    [SerializeField] RectTransform targetFlyRect;
    [SerializeField] RectTransform parentRect;
    [SerializeField] string[] encourageQuotes;
    public void SetCombo(int comboCount, int scoreBonus, Action complete)
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlayOneShot("Combo", 1f);
        comboText.gameObject.SetActive(true);
        encourageText.gameObject.SetActive(true);
        comboText.rectTransform.anchoredPosition = new Vector2(-1000, 30);
        encourageText.rectTransform.anchoredPosition = new Vector2(1000, -100);

        comboText.rectTransform.DOAnchorPosX(0, 0.25f).SetEase(Ease.OutBack);
        encourageText.rectTransform.DOAnchorPosX(0, 0.25f).SetEase(Ease.OutBack);

        comboText.text = "Combo X" + comboCount;
        int index = comboCount - 2;
        if (index >= encourageQuotes.Length)
        {
            index = encourageQuotes.Length - 1;
        }
        encourageText.text = encourageQuotes[index];
        Helper.CreateCounter(1.25f, () =>
        {
            comboText.gameObject.SetActive(false);
            encourageText.gameObject.SetActive(false);
            Vector2 anchorSpawn = DOTweenModuleUI.Utils.SwitchToRectTransform(GetComponent<RectTransform>(), parentRect);
            anchorSpawn = DOTweenModuleUI.Utils.SwitchToRectTransform(targetFlyRect, parentRect);
            // Butterfly bf = Instantiate(butterflyPrefab, parentRect);
            // bf.rect.anchoredPosition = anchorSpawn;
            // bf.SetDestination(anchorSpawn, complete);
            // bf.scoreText.text = $"+{scoreBonus}";
        });
    }
}
