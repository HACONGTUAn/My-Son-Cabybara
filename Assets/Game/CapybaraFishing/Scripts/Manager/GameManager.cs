using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Fishing
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public GameState gameState;
        public Action slashEvent,fishingEvent;
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
            SwitchGameState(GameState.Fishing);
        }
        public void SwitchGameState(GameState state)
        {
            gameState = state; 
            switch(gameState)
            {
                case GameState.Fishing:
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
        private void HandleFishingChange()
        {
            StartCoroutine(MoveCameraFishing());
            fishingEvent?.Invoke();
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

    }
    public enum GameState
    {
        Fishing,SlashFish
    }
}
