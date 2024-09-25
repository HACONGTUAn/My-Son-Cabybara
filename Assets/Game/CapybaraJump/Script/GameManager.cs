using CapybaraMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CapybaraJump
{
    public class GameManager : MonoBehaviour
    {
        public bool IsStart = false;
        public MiniGame2 minigame;
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
        public float jumpF = 12f;
        public float jumpTime = 0.3f;
        public int gravityScale = 3;
        public bool gameOver = false;
        public bool isJustShield = false;
        public GameObject oldCarpet;
        public GameObject floorCarpet;
        public int initSortingLayer;
        [SerializeField] private Vector3 camearaInitPosition;
        [SerializeField] private Vector3 capybaraInitPosition;
        [SerializeField] private Vector3 leftPosInitPosition;
        [SerializeField] private Vector3 rightPosInitPosition;
        [SerializeField] private GameObject startBtn;
        [SerializeField] private List<BoosterBtn> listItemsBtn;


        /// <summary>
        /// /
        /// </summary>


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
            foreach (BoosterBtn btn in listItemsBtn)
            {
                btn.resetStart();
            }
            IsStart = true;


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
            IsStart = false;
            gameOverPopUp.SetActive(true);
        }

        public void PlayAgain()
        {
            if (SpawnCarpet.Instance.transform.childCount > 0)
            {
                foreach (Transform carpet in SpawnCarpet.Instance.transform)
                {

                    carpet.gameObject.SetActive(false);

                }

            }
            
            CameraFollowController.Instance.transform.localPosition = camearaInitPosition;
            CameraFollowController.Instance.targetPos = camearaInitPosition;
            CapybaraMain.Instance.transform.localPosition = capybaraInitPosition;
            SpawnCarpet.Instance.spawnPosList[0].localPosition = leftPosInitPosition;
            CapybaraMain.Instance.animator.SetTrigger("idle");
            SpawnCarpet.Instance.spawnPosList[1].localPosition = rightPosInitPosition;
            ScoreController.Instance.ResetScore();
            oldCarpet = null;
            isBoost = false;
            isShield = false;
            jumpF = 12f;
            jumpTime = 0.3f;
            gameOver = false;
            isJustShield = false;
            startBtn.SetActive(true);
            gravityScale = 3;
            foreach (BoosterBtn btn in listItemsBtn)
            {
                btn.Refresh();
            }



            /*  StartGame();
             Time.timeScale = 1f; */
        }

        public void Booster()
        {
            if (isBoost)
            {
                return;
            }
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
