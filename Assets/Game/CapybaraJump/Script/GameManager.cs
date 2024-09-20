using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CapybaraJump
{
    public class GameManager : MonoBehaviour
    {
        public int CapybaraShield_LayerIndex = 8;
        public int CapybaraMain_LayerIndex = 7;
        public int CapybaraCarpet_LayerIndex = 6;
        public static GameManager Instance;
        public float angleCollisonEnterThreshHole = 35f;
        public float perfectJumpThreshHole = 3f;
        public GameObject gameOverPopUp;
        public CountDown countDown;
        public float startTime = 0.7f;
        public float endTime = 1.1f;
        public float fallTime = 0.5f;
        public bool isBoost = false;
        public bool isShield = false;
        public float jumpF = 30f;
        public float jumpTime = 0.3f;
        public bool gameOver = false;
        public bool isJustShield = false;
        public GameObject oldCarpet;
        public GameObject floorCarpet;


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            //StartCoroutine(StartPlay());
           // Time.timeScale = 0f;

        }

        private IEnumerator StartPlay()
        {
           // Time.timeScale = 1f;
            yield return new WaitForSeconds(1);
            countDown.gameObject.SetActive(true);
            countDown.StartCountdown();


        }
        public void StartGame()
        {
            StartCoroutine(StartPlay());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GameOver()
        {
           // Time.timeScale = 0f;
            gameOverPopUp.SetActive(true);
        }

        public void PlayAgain()
        {
            SceneManager.LoadScene(0);
            /*  StartGame();
             Time.timeScale = 1f; */
        }

        public void Booster()
        {
            isBoost = true;
          //  CapybaraMain.Instance.StopMove();
            CapybaraMain.Instance.MoveUpBooster(10);
        }

        public void Shield()
        {
            if (!isShield)
            {
                isShield = true;
                ScoreController.Instance.shield.SetActive(true);
            }
        }


    }


}
