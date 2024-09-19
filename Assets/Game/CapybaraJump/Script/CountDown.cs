using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CapybaraJump
{
    public class CountDown : MonoBehaviour
    {
        private float countdownTime = 5f; // Countdown time in seconds
        public TextMeshProUGUI timerText; // Reference to the UI Text component

        private float currentTime;

        void Start()
        {
            currentTime = countdownTime;
            //UpdateTimerText();
        }

        private IEnumerator Countdown()
        {
            float currentTime = countdownTime;

            while (currentTime > 0)
            {
                // Update the UI Text with the current time
                timerText.text = Mathf.Round(currentTime).ToString();

                // Wait for 1 second
                yield return new WaitForSeconds(1f);

                // Decrease the current time
                currentTime--;
            }

            // Ensure the timer shows 0 when done
            timerText.text = "";

            // Call a method to handle when the timer finishes
            OnCountdownFinished();
        }
        public void StartCountdown()
        {
            StartCoroutine(Countdown());
        }

        void OnCountdownFinished()
        {
            // Logic for when the countdown finishes
            SpawnCarpet.Instance.SpawnNewCarpet(2f);
            Debug.Log("Countdown Finished!");
            this.gameObject.SetActive(false);
        }
    }
}

