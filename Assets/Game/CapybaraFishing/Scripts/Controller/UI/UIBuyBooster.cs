using DG.Tweening;
using Fishing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
    public class UIBuyBooster : MonoBehaviour
    {
        private BoosterType boosterType;
        [SerializeField] private Button close, ticketBuy, adsBuy;
        [SerializeField] private Text priceText;
        [SerializeField] private Image boostImg;
        [SerializeField] private Sprite power,time;
        [SerializeField] private GameObject fakeAds;
        [SerializeField] private UIFishing uiFishing;
        [SerializeField] private RectTransform popunPanel;
        private BoosterController boosterController;       
        private int price;
        private void Start()
        {
            close.onClick.AddListener(OnCloseClick);
            ticketBuy.onClick.AddListener(OnTicketBuyClick);
            adsBuy.onClick.AddListener(OnAdsBuyClick);
            boosterController = GetComponent<BoosterController>();          
        }
        public void Initialize(BoosterType booster)
        {
            GameManager.Instance.PauseHandler();
            boosterType = booster;
            switch (boosterType)
            {
                case BoosterType.FishingPower:
                    boostImg.sprite = power;
                    priceText.text = "5";
                    break;
                case BoosterType.FishingTime:
                    boostImg.sprite = time;
                    priceText.text = "10";
                    break;
                default:
                    break;
            }            
            ShowPanel(popunPanel);

        }

        public void OnCloseClick()
        {
            GameManager.Instance.UnPauseHandler();
            uiFishing.UpadteUI();
            HidePanel(popunPanel);           
            
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
        private void OnTicketBuyClick()
        {
            boosterController.SetBooster(boosterType ,boosterController.GetBooster(boosterType) + 1);
        }
        private void OnAdsBuyClick()
        {
            boosterController.SetBooster(boosterType, boosterController.GetBooster(boosterType) + 1);
            fakeAds.SetActive(true);
        }

    }
}
