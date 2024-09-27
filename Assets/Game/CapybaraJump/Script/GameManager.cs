using CapybaraMain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CapybaraJump
{
    public class GameManager : MonoBehaviour
    {
        
        public bool IsStart = false;
        public MiniGame2 minigame;
        public int CapybaraMain_LayerIndex = 9;
        public static GameManager Instance;
        public float angleCollisonEnterThreshHole = 40f;
        public float perfectJumpThreshHole = 3f;

        public float besideThreshHole = 20f;
        public GameObject gameOverPopUp;
        public CountDown countDown;
        public float startTime = 0.7f;
        public float endTime = 1.0f;
        public float fallTime = 0.5f;
        public bool isBoost = false;
        public bool isShield = false;
        public float jumpF = 12f;
        public float jumpTime = 0.3f;
        public float gravityScale = 3;
        public bool gameOver = false;
        public bool isJustShield = false;
        public GameObject oldCarpet;
        public GameObject floorCarpet;
        public int initSortingLayer;

        
        
        [SerializeField] private GameObject PanelNewHighestScore;
        [SerializeField] private GameObject PanelScore;

        [SerializeField] private Text score;
        [SerializeField] private Text newHighestScore;
        [SerializeField] private Text highestScore;
        [SerializeField] private Vector3 capybaraInitPosition;
        [SerializeField] private Vector3 leftPosInitPosition;
        [SerializeField] private Vector3 rightPosInitPosition;
        [SerializeField] private GameObject startBtn;
        [SerializeField] private List<BoosterBtn> listItemsBtn;
         [SerializeField] private HeartAdsFill heart;


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
            Application.targetFrameRate = 60;

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
            if (ScoreController.Instance.score > PlayerPrefs.GetInt("highScore", 0))
            {
                
                PlayerPrefs.SetInt("highScore", ScoreController.Instance.score);
                PlayerPrefs.Save(); 
                PanelNewHighestScore.SetActive(true);
                PanelScore.SetActive(false);
                newHighestScore.text = PlayerPrefs.GetInt("highScore", 0) +"";
            }
            else{
                PanelNewHighestScore.SetActive(false);
                highestScore.text = PlayerPrefs.GetInt("highScore", 0) +"";
                score.text = ScoreController.Instance.score +"";
                PanelScore.SetActive(true);
            }
            // Time.timeScale = 0f;
            IsStart = false;
            gameOverPopUp.SetActive(true);
            heart.StartLoading();
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

            ScoreController.Instance.gift.gameObject.SetActive(false);
            CameraFollowController.Instance.ResetCamera();
            CapybaraMain.Instance.transform.localPosition = capybaraInitPosition;
            SpawnCarpet.Instance.spawnPosList[0].localPosition = leftPosInitPosition;
            CapybaraMain.Instance.animator.SetTrigger("touchGround");
            CapybaraMain.Instance.gameOverCalled = false;
            CapybaraMain.Instance.isMoving = false;
            CapybaraMain.Instance.transform.GetComponent<BoxCollider2D>().isTrigger = false;
            SpawnCarpet.Instance.spawnPosList[1].localPosition = rightPosInitPosition;
            ScoreController.Instance.ResetScore();
            oldCarpet = null;
            isBoost = false;
            isShield = false;
            //jumpF = 8f;
            jumpTime = 0.3f;
            gameOver = false;
            isJustShield = false;
            startBtn.SetActive(true);
            CapybaraMain.Instance.transform.GetComponent<Rigidbody2D>().gravityScale = 1f;
            gravityScale = 1;
            startTime = 0.7f;
            endTime = 1.0f;
            foreach (BoosterBtn btn in listItemsBtn)
            {
                btn.Refresh();
            }



            /*  StartGame();
             Time.timeScale = 1f; */
        }

        public void Booster()
        {
            if (isBoost || gameOver)
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
