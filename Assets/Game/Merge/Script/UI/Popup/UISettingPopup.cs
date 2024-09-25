using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Merge
{

    public class UISettingPopup : PopupUI
    {
        [SerializeField] Button soundButton;
        [SerializeField] Button musicButton;
        [SerializeField] Button vibratButton;
        [SerializeField] Button closeButton;
        [SerializeField] Button continueButton;
        [SerializeField] Button homeButton;
        [SerializeField] GameObject soundOjb, musicOjb, vibratOjb;
        private bool IsMusic, IsSound, IsVibrat;
        [SerializeField] GameObject settingContainer;
        public override void Initialize(UIManager manager)
        {
            base.Initialize(manager);
            soundButton.onClick.AddListener(Sound);
            musicButton.onClick.AddListener(Music);
            continueButton.onClick.AddListener(Continue);
            homeButton.onClick.AddListener(Home);
            vibratButton.onClick.AddListener(vibrat);
            closeButton.onClick.AddListener(Close);
            if(GameManager.Instance.currentMode == null) 
            { 
                continueButton.gameObject.SetActive(false);
            }
            AnimatedUI();
        }
        public override void Show(Action onClose)
        {
            base.Show(onClose);
            vibratOjb.SetActive(!IsVibrat);
            musicOjb.SetActive(!IsMusic);
            soundOjb.SetActive(!IsSound);
        }

        private void Continue()
        {
            uiManager.Loading();
            gameObject.SetActive(false);
        }
        private void Home()
        {
            Close();
            Capybara.GameManager.Instance.exit();
        }
        private void AnimatedUI()
        {
            settingContainer.transform.localScale = Vector3.zero;
            settingContainer.transform.DOScale(Vector3.one, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
        }

        private void vibrat()
        {
            IsVibrat = !IsVibrat;
            vibratOjb.SetActive(!IsVibrat); 
        }

        private void Music()
        {
            IsMusic = !IsMusic;
            AudioManager.musicVolume = IsMusic? 1: 0;
            musicOjb.SetActive(!IsMusic);
        }
        private void Sound()
        {
            IsSound = !IsSound;
            AudioManager.soundVolume = IsSound? 1: 0;
            soundOjb.SetActive(!IsSound);
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

}