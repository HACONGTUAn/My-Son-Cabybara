using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CapybaraMain;
namespace CapybaraJump
{
    public class BuyBooster : MonoBehaviour
    {
        [SerializeField] Button exitButton, buyCoin, buyAD;
        [SerializeField] Text coinCount, heartCount;
        public BoosterType boosterType;
        public bool isBuy;
        [SerializeField] GameObject[] boosterImgs;
        [SerializeField] BoosterBtn[] boosterBtns;
        [SerializeField] Transform popup;
        [SerializeField] BoosterSO boosterSO;
        [SerializeField] UnCoinFx unCoinFx;
        private void Start()
        {
            isBuy = false;
            exitButton.onClick.AddListener(Close);
            buyCoin.onClick.AddListener(BuyCoin);  
            buyAD.onClick.AddListener(BuyAD);
            UpdateCount();
            AnimatedUI();
        }

        public void UpdateCount()
        {
            boosterImgs[(int)boosterType].SetActive(true);
            buyCoin.transform.GetChild(0).GetComponent<Text>().text = boosterSO.GetBoosterPrice(boosterType).ToString();
            coinCount.text = Manager.Instance.GetTicket().ToString();
            heartCount.text = Manager.Instance.GetHeart().ToString();    
            buyCoin.interactable = Manager.Instance.GetTicket() >= boosterSO.GetBoosterPrice(boosterType);
        }
        private void BuyCoin()
        {
            if(Manager.Instance.GetTicket() >= boosterSO.GetBoosterPrice(boosterType) && !isBuy)
            {
                isBuy = true;
                unCoinFx.PlayFx(() =>
                {
                    isBuy = false;
                    Manager.Instance.SetTicket(Manager.Instance.GetTicket() - 1);
                    UpdateCount();
                }, 0, buyCoin.transform, boosterSO.GetBoosterPrice(boosterType));
                // Manager.Instance.SetTicket(Manager.Instance.GetTicket() - boosterSO.GetBoosterPrice(boosterType));
                if ((int)boosterType == 0)
                {
                    GameManager.Instance.minigame.items[0].quantity += 1;
                }
                if ((int)boosterType == 1)
                {

                    GameManager.Instance.minigame.items[1].quantity += 1;
                }
                boosterBtns[(int)boosterType].Refresh();
            }
        }
        private void BuyAD()
        {

            if(true)
            {
                // Manager.Instance.SetTicket(Manager.Instance.GetTicket() - boosterSO.GetBoosterPrice(boosterType));
                if ((int)boosterType == 0)
                {
                    GameManager.Instance.minigame.items[0].quantity += 1;
                }
                if ((int)boosterType == 1)
                {

                    GameManager.Instance.minigame.items[1].quantity += 1;
                }
                boosterBtns[(int)boosterType].Refresh();
            }
        }

        public void AnimatedUI()
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
                boosterImgs[(int)boosterType].SetActive(false);
                boosterBtns[(int)boosterType].startBtn.SetActive(true);
                gameObject.SetActive(false);
            });
        }
    }
}