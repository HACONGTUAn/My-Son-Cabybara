using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
namespace Merge
{
    public class UILoseScreen : ScreenUI
    {
        [SerializeField] Button restartButton;
        [SerializeField] Button homeButton;
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
            restartButton.onClick.AddListener(Restart);
            homeButton.onClick.AddListener(Home);
        }
        public override void Active()
        {
            base.Active();
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
            if (score > highScore)
            {
                
            }
            else
            {
                
            }
        }
        private void Restart()
        {
            reviveCount = 0;
            reviveCallBack?.Invoke(false);
            GameManager.Instance.Resume();
        }
        private void Home()
        {
            Restart();
            Capybara.GameManager.Instance.exit();
        }
    }

}