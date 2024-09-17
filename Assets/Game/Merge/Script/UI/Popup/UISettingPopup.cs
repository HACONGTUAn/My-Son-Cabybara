using DG.Tweening;
using mygame.sdk;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UISettingPopup : PopupUI
{
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Button rateButton;
    [SerializeField] Button closeButton;
    [SerializeField] Button removeAdsButton;
    [SerializeField] Button mainMenuButton;
    // [SerializeField] AdventureMode adventureMode;
    [SerializeField] GameObject settingContainer;
    public override void Initialize(UIManager manager)
    {
        base.Initialize(manager);
        soundSlider.onValueChanged.AddListener(Sound);
        musicSlider.onValueChanged.AddListener(Music);
        removeAdsButton.onClick.AddListener(RemoveAds);
        mainMenuButton.onClick.AddListener(BackToMainMenu);
        rateButton.onClick.AddListener(Rate);
        closeButton.onClick.AddListener(Close);
        if (GameManager.Instance.currentMode)
        {
            // adventureMode = GameManager.Instance.currentMode.GetComponent<AdventureMode>();
        }
        soundSlider.value = AudioManager.soundVolume;
        musicSlider.value = AudioManager.musicVolume;
        if(GameManager.Instance.currentMode == null) 
        { 
            mainMenuButton.gameObject.SetActive(false);
            rateButton.transform.GetComponent<RectTransform>().localPosition = new Vector3(16.8600006f, -197, 0);
        }
        AnimatedUI();
    }
    public override void Show(Action onClose)
    {
        base.Show(onClose);
        // if (AdsHelper.isRemoveAds(0))
        // {
        //     removeAdsButton.gameObject.SetActive(false);
        // }
    }
    private void RemoveAds()
    {
        // InappHelper.Instance.BuyPackage("removeads", "setting", (cb) =>
        // {
        //     if (cb.status == 1)
        //     {
        //         removeAdsButton.gameObject.SetActive(false);
        //     }
        // });
    }

    private void BackToMainMenu()
    {
        uiManager.Loading();
        gameObject.SetActive(false);
    }
    private void AnimatedUI()
    {
        settingContainer.transform.localScale = Vector3.zero;
        settingContainer.transform.DOScale(Vector3.one, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
    }

    private void Rate()
    {
        // SDKManager.Instance.showRate();
    }

    private void Music(float val)
    {
        AudioManager.musicVolume = val;
    }
    private void Sound(float val)
    {
        AudioManager.soundVolume = val;
    }
    private void Close()
    {
        settingContainer.transform.DOScale(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.InBack).OnComplete(() =>
        {
            if (Time.timeScale == 0) { Time.timeScale = 1; }
            Hide();
        });
    }
}
