using Fishing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing {
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Text timerText;
        [SerializeField] private Button restart;
        [SerializeField] private GameObject tapToStart;
        public int timer = 30;
        private void Start()
        {
            restart.onClick.AddListener(OnRestartClick);
        }
        private void Update()
        {
            if (GameManager.Instance.gameState == GameState.Start)
            {
                if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
                {
                    GameManager.Instance.SwitchGameState(GameState.Fishing);
                    tapToStart.SetActive(false);
                }
            }
        }
        private void OnRestartClick()
        {
            GameManager.Instance.SwitchGameState(GameState.Fishing);
        }
        public void TimmerCount()
        {
            StartCoroutine(StartTimerCount());
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