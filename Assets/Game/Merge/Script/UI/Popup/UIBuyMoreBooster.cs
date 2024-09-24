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
        [SerializeField] Text coinCount, heartCount;
        public EBoosterType boosterType;
        public bool isBuy;
        [SerializeField] GameObject[] boosterImgs;
        [SerializeField] Transform popup;
        [SerializeField] BoosterSO boosterSO;
        [SerializeField] UnCoinFx unCoinFx;
        public override void Initialize(UIManager manager)
        {
            base.Initialize(manager);
            exitButton.onClick.AddListener(Close);
            buyCoin.onClick.AddListener(BuyCoin);  
            buyAD.onClick.AddListener(BuyAD);
            UpdateCount();
            isBuy = false;
        }
        private void Start()
        {
            boosterImgs[(int)boosterType].SetActive(true);
            buyCoin.transform.GetChild(0).GetComponent<Text>().text = boosterSO.GetBoosterPrice(boosterType).ToString();
            buyCoin.interactable = CapybaraMain.Manager.Instance.GetTicket() >= boosterSO.GetBoosterPrice(boosterType);
            AnimatedUI();
        }

        private void UpdateCount()
        {
            coinCount.text = CapybaraMain.Manager.Instance.GetTicket().ToString();
            heartCount.text = CapybaraMain.Manager.Instance.GetHeart().ToString();     
        }
        private void BuyCoin()
        {
            if(CapybaraMain.Manager.Instance.GetTicket() >= boosterSO.GetBoosterPrice(boosterType) && !isBuy)
            {
                isBuy = true;
                unCoinFx.PlayFx(() =>
                {
                    isBuy = false;
                    CapybaraMain.Manager.Instance.SetTicket(CapybaraMain.Manager.Instance.GetTicket() - 1);
                    UpdateCount();
                }, 0, buyCoin.transform, boosterSO.GetBoosterPrice(boosterType));
                // CapybaraMain.Manager.Instance.SetTicket(CapybaraMain.Manager.Instance.GetTicket() - boosterSO.GetBoosterPrice(boosterType));
                if ((int)boosterType == 0)
                {
                    GameManager.Instance.minigame.items[0].quantity += 1;
                }
                if ((int)boosterType == 6)
                {

                    GameManager.Instance.minigame.items[1].quantity += 1;
                }
                Observer.Notify(UIBoosterPanel.RefreshUseBoosterKey);
            }
        }
        private void BuyAD()
        {

            //GameRes.AddRes(new DataTypeResource(RES_type.BOOSTER, (int)boosterType), 1, "Buy by AD");
            if ((int)boosterType == 0)
            {
                GameManager.Instance.minigame.items[0].quantity += 1;
            }
            if ((int)boosterType == 6)
            {

                GameManager.Instance.minigame.items[1].quantity += 1;
            }
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