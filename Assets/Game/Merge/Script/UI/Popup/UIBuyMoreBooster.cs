using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Merge
{
    public class UIBuyMoreBooster : PopupUI
    {
        [SerializeField] Button exitButton, buyCoin, buyAD;
        [SerializeField] Text coinCount, heartCount, heartCDTexts;
        public EBoosterType boosterType;
        [SerializeField] GameObject[] boosterImgs;
        [SerializeField] Transform popup;
        [SerializeField] BoosterSO boosterSO;
        public override void Initialize(UIManager manager)
        {
            exitButton.onClick.AddListener(Close);
            buyCoin.onClick.AddListener(BuyCoin);  
            buyAD.onClick.AddListener(BuyAD);
            UpdateCount();
            base.Initialize(manager);
        }

        void Update()
        {
            heartCDTexts.text = GameManager.Instance.heartManager.GetTimeRemaningText();
        }
        private void Start()
        {
            boosterImgs[(int)boosterType].SetActive(true);
            buyCoin.transform.GetChild(0).GetComponent<Text>().text = boosterSO.GetBoosterPrice(boosterType).ToString();
            buyCoin.interactable = DataManager.Coin >= boosterSO.GetBoosterPrice(boosterType);
            AnimatedUI();
        }

        private void UpdateCount()
        {
            coinCount.text = DataManager.Coin.ToString();
            heartCount.text = DataManager.Heart.ToString();     
        }
        private void BuyCoin()
        {
            if(DataManager.Coin >= boosterSO.GetBoosterPrice(boosterType))
            {
                DataManager.DecreaseCoin(boosterSO.GetBoosterPrice(boosterType));
                GameRes.AddRes(new DataTypeResource(RES_type.BOOSTER, (int)boosterType), 1, "Buy by coin");
                Observer.Notify(UIBoosterPanel.RefreshUseBoosterKey);
                UpdateCount();
            }

        }
        private void BuyAD()
        {

            GameRes.AddRes(new DataTypeResource(RES_type.BOOSTER, (int)boosterType), 1, "Buy by AD");
            Observer.Notify(UIBoosterPanel.RefreshUseBoosterKey);     
        }

        private void AnimatedUI()
        {
            popup.localScale = Vector3.zero;
            popup.DOScale(Vector3.one, 0.5f).SetUpdate(true).SetEase(Ease.OutBack);
        }
        private void Close()
        {
            popup.DOScale(Vector3.zero, 0.5f).SetUpdate(true).SetEase(Ease.InBack).OnComplete(() =>
            {
                if (Time.timeScale == 0) { Time.timeScale = 1; }
                // if(GameManager.Instance.currentMode.GetComponent<AdventureMode>() != null)
                // {
                //     GameManager.Instance.currentMode.GetComponent<AdventureMode>().pauseTimer = false;  
                // }
                Hide();
            });
        }
    }
}