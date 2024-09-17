using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CapybaraJump
{
    public class GameManager : MonoBehaviour
    {
        public int CapybaraMain_LayerIndex = 7;
        public int CapybaraCarpet_LayerIndex = 6;
        public static GameManager Instance;
        public  float angleCollisonEnterThreshHole = 50f;
        public GameObject gameOverPopUp;

        void Awake(){
           if(Instance == null){
                Instance = this;
           }
        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(StartPlay());
            
        }

        private  IEnumerator StartPlay(){
            yield return new WaitForSeconds(4);
            SpawnCarpet.Instance.SpawnNewCarpet();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void GameOver(){
            Time.timeScale = 0f;
            gameOverPopUp.SetActive(true);
        }

        public void PlayAgain(){
            SceneManager.LoadScene(0);
             Time.timeScale = 1f;
        }


    }


}
