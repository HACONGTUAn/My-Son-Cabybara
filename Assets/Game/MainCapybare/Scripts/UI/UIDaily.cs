using System;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework.Internal;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Capybara
{
    public class UIDaily : MonoBehaviour
    {
        public CoinFx coinFx;
        public Button claimButton;
        public Button claimButtonX2;
        public Sprite image1;
        public Sprite image2;
        public Sprite image3;
        public Sprite image4;
        public DataDaily dataDaily;
        public List<DailyClaim> dailyClaims;
        public void Start()
        {
            if(dataDaily.dayGame < 1 || dataDaily.dayGame > 7)dataDaily.dayGame = 1;
            claimButton.onClick.AddListener(ClaimReward);
            claimButtonX2.onClick.AddListener(ClaimRewardX2);
            CheckClaim();
            CheckDay();
        }
        private void Update() 
        {
            CheckDay();
        }
        private void ClaimReward()
        {
            if(!dataDaily.daily[dataDaily.dayGame-1].isUnlocked)
            {
                dataDaily.daily[dataDaily.dayGame-1].isUnlocked = true;
                UnlockButton();
                coinFx.PlayFx(Claim, 0, dailyClaims[dataDaily.dayGame-1].fx.position, dataDaily.daily[dataDaily.dayGame-1].ticket );

            }
        }
        private void ClaimRewardX2()
        {
            if(!dataDaily.daily[dataDaily.dayGame-1].isUnlocked)
            {
                dataDaily.daily[dataDaily.dayGame-1].isUnlocked = true;
                UnlockButton();
                coinFx.PlayFx(Claim, 0, dailyClaims[dataDaily.dayGame-1].fx.position, dataDaily.daily[dataDaily.dayGame-1].ticket *2 );
            }
        }
        private void Claim()
        {
            dailyClaims[dataDaily.dayGame-1].reward.SetActive(true);
            CapybaraMain.Manager.Instance.SetTicket(CapybaraMain.Manager.Instance.GetTicket() + 1);
            CapybaraMain.HearTicker.Instance.ChangeValue();
        }
        private void CheckDay()
        {
            if (System.DateTime.Now.Day == dataDaily.dayReal)
            {
                return;
            }
            else
            {
                dataDaily.dayReal = System.DateTime.Now.Day;
                if(dataDaily.dayGame < 1)dataDaily.dayGame = 1;
                if(dataDaily.daily[dataDaily.dayGame-1].isUnlocked)
                {
                    dataDaily.dayGame ++;
                    if (dataDaily.dayGame > 7)
                    {
                        dataDaily.dayGame = 1;
                        foreach (Daily daily in dataDaily.daily)
                        {
                            daily.isUnlocked = false;
                        }
                    }
                }
            }
            CheckClaim();
        }
        private void CheckClaim()
        {
            int min = math.min(dailyClaims.Count, dataDaily.daily.Count);
            for(int i = 0; i < min; i++)
            {
                if(i<dataDaily.dayGame-1)
                {
                    dataDaily.daily[i].isUnlocked = true;
                }
                dailyClaims[i].reward.SetActive(false);
                if(dataDaily.daily[i].isUnlocked)
                {
                    dailyClaims[i].reward.SetActive(true);
                }
                dailyClaims[i].outline.SetActive(false);
                dailyClaims[i].day.text = "Day " + (i+1);
                dailyClaims[i].ticket.text = "X" + dataDaily.daily[i].ticket.ToString();
                dailyClaims[i].image1.sprite = image3;
                dailyClaims[i].image2.sprite = image4;
                if(i+1 == dataDaily.dayGame)
                {
                    dailyClaims[i].image1.sprite = image1;
                    dailyClaims[i].image2.sprite = image2;
                    dailyClaims[i].day.text = "Today";
                    dailyClaims[i].outline.SetActive(true);
                }
            }
            UnlockButton();
        }
        private void UnlockButton()
        {
            if(dataDaily.daily[dataDaily.dayGame-1].isUnlocked)
            {
                claimButton.interactable = false;
                claimButtonX2.interactable = false;
            }
            else
            {
                claimButton.interactable = true;
                claimButtonX2.interactable = true;
            }
        }
        [System.Serializable]
        public class DailyClaim
        {
            public int id;
            public GameObject reward;
            public GameObject outline;
            public UnityEngine.UI.Image image1;
            public UnityEngine.UI.Image image2;
            public Text day;
            public Text ticket;
            public Transform fx;
        }
    }
}