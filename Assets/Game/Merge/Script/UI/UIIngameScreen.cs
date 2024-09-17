using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using mygame.sdk;
using Random = UnityEngine.Random;
public class UIIngameScreen : ScreenUI
{
    [SerializeField] UIBoosterPanel boosterPanel;
    [SerializeField] Button pauseButton;
    // [SerializeField] Button removeAdsButton;
    [SerializeField] Image nextFruitIconImage;
    [SerializeField] Text scoreText;
    [SerializeField] Text scoreNextText;
    [SerializeField] UIComboAnimation comboAnimation;
    [SerializeField] ItemObjectContainerSO fruitContainer;
    private Tween scoreTween;
    private Tween scaleTween;
    public override void Initialize(UIManager uiManager)
    {
        base.Initialize(uiManager);
        boosterPanel.Initialize();
        pauseButton.onClick.AddListener(Pause);
        // removeAdsButton.onClick.AddListener(RemoveAds);
    }

    private void RemoveAds()
    {
        // uiManager.ShowPopup<UINoAdsPopup>(null);
        //InappHelper.Instance.BuyPackage("removeads", "ingame", (cb) =>
        //{
        //    if (cb.status == 1)
        //    {
        //        removeAdsButton.gameObject.SetActive(false);
        //    }
        //});
    }

    public override void Active()
    {
        base.Active();
        // ClassicMode.OnMergeFruit += OnMerge;
        // comboText.gameObject.SetActive(false);
        // removeAdsButton.gameObject.SetActive(!AdsHelper.isRemoveAds(1));
        boosterPanel.Refresh();
    }
    public void ShowNative()
    {
        //nativeAdObj.SetActive(true);
    }
    public override void Deactive()
    {
        // ClassicMode.OnMergeFruit -= OnMerge;
        base.Deactive();
    }
    // private void OnMerge(Fruit arg1, Fruit arg2, int comboCount)
    // {
    //     if (comboCount >= 3 && comboCount % 2 != 0)
    //     {
    //         Vector2 anchorSpawn = uiManager.canvas.WorldToCanvasPosition(arg1.position);
    //         Butterfly bf = Instantiate(butterflyPrefab, transform);
    //         bf.rect.anchoredPosition = anchorSpawn;
    //         // anchorSpawn = DOTweenModuleUI.Utils.SwitchToRectTransform(scoreText.rectTransform, GetComponent<RectTransform>());
    //         float x = Screen.width * (0.6f) * (leftSide ? -1 : 1);
    //         leftSide = !leftSide;
    //         anchorSpawn = new Vector2(x, anchorSpawn.y + Random.Range(300, 500));
    //         bf.SetDestination(anchorSpawn);
    //     }
    //     if (comboCount >= 2)
    //     {
    //         comboText.gameObject.SetActive(true);
    //         comboText.text = "x" + comboCount;
    //         comboText.transform.localScale = Vector3.one;
    //         timer = 3f;
    //         if (tweenCombo != null)
    //         {
    //             tweenCombo.Kill();
    //         }
    //         comboText.transform.localScale = Vector3.zero;
    //         tweenCombo = comboText.transform.DOScale(Vector3.one * 1.4f, 0.25f).SetEase(Ease.OutCubic).OnComplete(() =>
    //         {
    //             tweenCombo = comboText.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutCubic);
    //         });
    //     }
    // }
    public void SetCombo(int count, int scoreBonus, Action complete)
    {
        comboAnimation.SetCombo(count, scoreBonus, complete);
    }

    private void Pause()
    {
        GameManager.Instance.Pause();
        uiManager.ShowPopup<UISettingPopup>(() =>
        {
            GameManager.Instance.Resume();
        });
    }
    public void SetScore(int currentScore, int nextScore)
    {
        if (currentScore == nextScore)
        {
            scoreText.text = currentScore.ToString();
            return;
        }
        int val = currentScore;
        if (scoreTween != null)
        {
            scoreTween.Kill(true);
        }
        if (scaleTween != null)
        {
            scaleTween.Kill();
        }
        scoreNextText.text = "+" + (nextScore - currentScore).ToString();
        scaleTween = scoreNextText.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
        {
            scaleTween = scoreNextText.transform.DOScale(Vector3.zero, 0.25f).SetDelay(1f);
        });
        scoreTween = DOTween.To(() => val, x => val = x, nextScore, 1f).OnUpdate(() =>
        {
            scoreText.text = val.ToString();
        });
    }
    public void SetNextFruit(FruitType fruitType)
    {
        FruitItemObjectSO fruitInfo = null;
        for (int i = 0; i < fruitContainer.container.Length; i++)
        {
            FruitItemObjectSO f = fruitContainer.container[i] as FruitItemObjectSO;
            if (f.fruitType == fruitType)
            {
                fruitInfo = f;
                break;
            }
        }
        nextFruitIconImage.sprite = fruitInfo.icon;
    }
    protected override void OnScreenDestroyed()
    {
        boosterPanel.Dispose();
        base.OnScreenDestroyed();
    }
}
