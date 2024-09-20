using System;
using System.Collections;
using UnityEngine;

namespace Fishing
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public GameState gameState;
        public Action slashEvent,fishingEvent;
        private UIController uiController;
        private AquariumControler aquariumControler;
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }
        private void OnEnable()
        {
            gameState = GameState.Start;
        }
        void Start()
        {
            Initialize();
        }     
        private void Initialize()
        {
            uiController = GetComponent<UIController>();    
            aquariumControler = GetComponent<AquariumControler>();
        }
        public void SwitchGameState(GameState state)
        {
            gameState = state; 
            switch(gameState)
            {
                case GameState.Fishing:
                    ClearMap();
                    HandleFishingChange();
                    break;
                case GameState.SlashFish:
                    HandleSlashFishChange();
                    break;
            }
        }
        private void HandleSlashFishChange()
        {
            StartCoroutine(MoveCameraSlash());
            slashEvent?.Invoke();
            
        }
        private void HandleFishingChange()
        {
            //aquariumControler.SpawnFishHandler();
            uiController.timer = 30;
            uiController.TimmerCount();
            StartCoroutine(MoveCameraFishing());
            fishingEvent?.Invoke();
        }

        IEnumerator MoveCameraSlash()
        {
            Vector3 targetPosition = new Vector3(0, 9, -10);
            Camera cam = Camera.main;
            while (Vector3.Distance(cam.transform.position, targetPosition) > 0.05f)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, 2f * Time.deltaTime);
                yield return null;
            }
            cam.transform.position = targetPosition;
        }      
        IEnumerator MoveCameraFishing()
        {
            Vector3 targetPosition = new Vector3(0, 0, -10);
            Camera cam = Camera.main;
            while (Vector3.Distance(cam.transform.position, targetPosition) > 0.1f)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, 1.5f * Time.deltaTime);
                yield return null;
            }
            cam.transform.position = targetPosition;
        }
        private void ClearMap()
        {

        }
    }
    public enum GameState
    {
        Start,Fishing,SlashFish
    }
}
