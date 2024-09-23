using Fishing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
    public class UIFishing : MonoBehaviour, IBaseUI
    {
        [SerializeField] private Text timerText,powerNum,timeNum;
        [SerializeField] private Button boosterPower, boosterTime;
        [SerializeField] private GameObject plusPower, plusTime;
        [SerializeField] private UIBuyBooster uiBuyBooster;

        private BoosterController boosterController;
        private Coroutine timeCount;
        private int timer,powerBoost,timeBoost;
        void Awake()
        {
            boosterController = GetComponent<BoosterController>();
            boosterPower.onClick.AddListener(OnBoosterPowerClick);
            boosterTime.onClick.AddListener(OnBoosterTimeClick);
        }
        public void Initialize()
        {
            timer = 60;
            gameObject.SetActive(true);
            timeCount = StartCoroutine(StartTimerCount());
            UpadteUI();
        }
        public void UpadteUI()
        {
            powerBoost = boosterController.GetBooster(BoosterType.FishingPower);
            timeBoost = boosterController.GetBooster(BoosterType.FishingTime);
            if (powerBoost == 0)
            {
                plusPower.SetActive(true);
            }
            else
            {
                plusPower.SetActive(false);
                powerNum.text = powerBoost.ToString();
            }
            if (timeBoost == 0)
            {

                plusTime.SetActive(true);
            }
            else
            {
                plusTime.SetActive(false);
                timeNum.text = timeBoost.ToString();
            }
        }
        public void Clear()
        {
            if (timeCount != null) StopCoroutine(timeCount);
            gameObject.SetActive(false);
        }
        private void OnBoosterPowerClick()
        {
            int numBooster = boosterController.GetBooster(BoosterType.FishingPower);
            if (numBooster > 0)
                UsePowerBooster(numBooster);
            else
                uiBuyBooster.Initialize(BoosterType.FishingPower);
            UpadteUI();
        }
        private void OnBoosterTimeClick()
        {
            int numBooster = boosterController.GetBooster(BoosterType.FishingTime);
            if (numBooster > 0)
                UseTimeBooster(numBooster);
            else
                uiBuyBooster.Initialize(BoosterType.FishingTime);
            UpadteUI();
        }
        private void UsePowerBooster(int numBooster)
        {
            boosterController.SetBooster(BoosterType.FishingPower, numBooster - 1);
            HookController.Instance.OnPowerUse();
            Debug.Log("Use Power");
        }
        private void UseTimeBooster(int numBooster)
        {
            boosterController.SetBooster(BoosterType.FishingTime, numBooster - 1);
            timer += 10;
            Debug.Log("Use Time");
        }

        private IEnumerator StartTimerCount()
        {
            timerText.text = timer + "";
            while (timer > 0)
            {
                timer--;
                yield return new WaitForSeconds(1f);
                timerText.text = timer + "";                
            }

            GameManager.Instance.SwitchGameState(GameState.SlashFish);
        }

    }
}
