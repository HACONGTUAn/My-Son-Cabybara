using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Fishing;

namespace Fishing
{
    public class UIEnd : MonoBehaviour
    {      
        [SerializeField] private RectTransform panelPopup;
        [SerializeField] private Text highestScoreText;
        [SerializeField] private Text curScoreText;
        [SerializeField] private Text reciveHeartText;
        [SerializeField] private Button recive, adsX2;
        private int highestScore, curScore, reciveHeart;
       
        void Start()
        {
            recive.onClick.AddListener(OnReciveClick);
            adsX2.onClick.AddListener(OnAdsX2Click);
        }
        public void Initialize(int score)
        {
            curScore = score;
            curScoreText.text = curScore.ToString();
            highestScore = Mathf.Max(GetHighestScore(), curScore);
            highestScoreText.text = "Highest Score : " + highestScore.ToString();
            SetHighestScore(highestScore);
            ShowPanel(panelPopup);
            UpdateHeart();
        }
        private void UpdateHeart()
        {
            // Do thing update heart
        }
        private int GetHighestScore()
        {
            return PlayerPrefs.GetInt("FishingHighest", 0);
        }
        private void SetHighestScore(int high)
        {
            PlayerPrefs.SetInt("FishingHighest", high);
        }
        private void OnReciveClick()
        {
            // Do add heart
            HidePanel(panelPopup);
            GameManager.Instance.SwitchGameState(GameState.Start);
        }
        private void OnAdsX2Click()
        {
            // Do x2 heart then add heart
            HidePanel(panelPopup);
            GameManager.Instance.SwitchGameState(GameState.Start);
        }

        public void ShowPanel(RectTransform panel)
        {
            gameObject.SetActive(true);
            panel.localScale = Vector3.zero;
            panel.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        public void HidePanel(RectTransform panel)
        {
            panel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
            gameObject.SetActive(false);
        }
    }
}