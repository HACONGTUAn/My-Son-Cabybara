using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing
{
    public class UISlash : MonoBehaviour, IBaseUI
    {
        public static UISlash Instance;
        [SerializeField] private Text scoreText;
        [SerializeField] private UIEnd uiEnd;
        private int score = 0;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        public void Initialize()
        {
            score = 0;
            scoreText.text = "0";
            gameObject.SetActive(true);

        }
        public void Clear()
        {

            gameObject.SetActive(false);
        }
        public void UpdateScore(int addNum)
        {
            score += addNum;
            score = Mathf.Max(score, 0);
            scoreText.text = score + "";
        }
        public void EndGame()
        {
            uiEnd.Initialize(score);
        }
    }
}
