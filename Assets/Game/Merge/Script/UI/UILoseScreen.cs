using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using mygame.sdk;
using Spine.Unity;
public class UILoseScreen : ScreenUI
{
    [SerializeField] SkeletonGraphic highScoreTitle;
    [SerializeField] SkeletonGraphic loseTitle;
    [SerializeField] Button continueButton;
    [SerializeField] Button restartButton;
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;
    [SerializeField] CanvasGroup canvasGroup;
    // [SerializeField] AdsNativeObject nativeAdObj;
    private event Action<bool> reviveCallBack;
    private int reviveCount
    {
        get { return PlayerPrefs.GetInt("revive_count", 0); }
        set { PlayerPrefs.SetInt("revive_count", value); }
    }
    public override void Initialize(UIManager uiManager)
    {
        base.Initialize(uiManager);
        continueButton.onClick.AddListener(Continue);
        restartButton.onClick.AddListener(Restart);
    }
    public override void Active()
    {
        base.Active();
        continueButton.gameObject.SetActive(reviveCount == 0);
        // nativeAdObj.gameObject.SetActive(false);
        // float aspectOri = 1080f / 1920f;
        // float newAspect = (float)Screen.width / (float)Screen.height;
        // float multiply = aspectOri / newAspect;
        // float newSize = 450 * multiply;
        // float yHeight = (newSize - 450) / 2f;
        // nativeAdObj.sizeDelta = new Vector2(nativeAdObj.sizeDelta.x, newSize);
        // nativeAdObj.anchoredPosition = new Vector2(nativeAdObj.anchoredPosition.x, nativeAdObj.anchoredPosition.y + yHeight);
        // AdsHelper.Instance.showNative("lose", nativeAdObj, false, false, (status) =>
            // {
            //     if (status == AD_State.AD_LOAD_OK)
            //     {
            //         nativeAdObj.gameObject.SetActive(true);
            //     }
            // });
    }
    public void SetUp(int score, int highScore, Action<bool> revive)
    {
        reviveCallBack = revive;
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f).SetEase(Ease.Linear);
        if (DataManager.HighScoreClassicMode < score)
        {
            DataManager.HighScoreClassicMode = score;
        }
        scoreText.text = score.ToString();
        highScoreText.text = DataManager.HighScoreClassicMode.ToString();
        highScoreTitle.gameObject.SetActive(score > highScore);
        loseTitle.gameObject.SetActive(score <= highScore);
        if (score > highScore)
        {
            highScoreTitle.AnimationState.AddAnimation(0, "Show", false, 0.25f);
            highScoreTitle.AnimationState.AddAnimation(0, "Idle", true, -1);
        }
    }
    private void Restart()
    {
        reviveCount = 0;
        reviveCallBack?.Invoke(false);
        //AdsHelperWrapper.ShowFull("restart");
    }
    private void Continue()
    {
        reviveCount = 0;
        reviveCallBack?.Invoke(false);
        // AdsHelperWrapper.ShowGift("revive", (cb) =>
        // {
        //     if (cb == AD_State.AD_REWARD_OK)
        //     {
        //         reviveCount++;
        //         reviveCallBack?.Invoke(true);
        //     }
        // });
    }
}
