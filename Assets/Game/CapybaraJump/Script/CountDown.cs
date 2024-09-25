using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CapybaraJump
{
    public class CountDown : MonoBehaviour
    {
        //abc
        //abc
        private float countdownTime = 3f; // Countdown time in seconds
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
            OnRaycast();
        }
        public void OnRaycast()
        {
            transform.parent.GetComponent<GraphicRaycaster>().enabled = true;
        }

        public void OffRaycast()
        {
            transform.parent.GetComponent<GraphicRaycaster>().enabled = false;
        }
    }
}

