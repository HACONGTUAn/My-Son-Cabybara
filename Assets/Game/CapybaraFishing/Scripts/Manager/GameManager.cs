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
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }       
        void Start()
        {
            Initialize();
        }     
        private void Initialize()
        {
            Application.targetFrameRate = 60;
            uiController = GetComponent<UIController>();              
        }
        public void SwitchGameState(GameState state)
        {
            gameState = state; 
            switch(gameState)
            {
                case GameState.Start:
                    HandleStartChange();
                    break;
                case GameState.Fishing:
                    ClearMap();
                    HandleFishingChange();
                    break;
                case GameState.SlashFish:
                    HandleSlashFishChange();
                    break;
            }
        }
        private void HandleStartChange() 
        { 
            uiController.SwitchUIState(UIState.Start);
            StartCoroutine(MoveCameraFishing());
        }
        private void HandleFishingChange()
        {                    
            uiController.SwitchUIState(UIState.Fishing);
            StartCoroutine(MoveCameraFishing());
            fishingEvent?.Invoke();
        }
        
        private void HandleSlashFishChange()
        {
            uiController.SwitchUIState(UIState.Slashing);
            StartCoroutine(MoveCameraSlash());
            slashEvent?.Invoke();
            
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
