using Fishing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fishing {
    public class UIController : MonoBehaviour
    {
         public CapybaraMain.MiniGame3 _dataMiniGame;
        [SerializeField] Text heartText;
        [SerializeField] Text ticketText;
        public static UIController Instance;
        [SerializeField] private Button touchDetect;
        [SerializeField] private UIStart uiStart;     
        [SerializeField] private UIFishing uiFishing;
        [SerializeField] private UISlash uiSlash;                                
        public UIState state;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }           
        }
        private void Start()
        {
            
            touchDetect.onClick.AddListener(OnTouchDetectClick);
            UpdateHeartAndTicket();
        }
        public void UpdateHeartAndTicket()
        {
            heartText.text = CapybaraMain.Manager.Instance.GetHeart().ToString();
            ticketText.text = CapybaraMain.Manager.Instance.GetTicket().ToString();
        }
        private void OnTouchDetectClick()
        {
            if(GameManager.Instance.gameState == GameState.Start)
            {
                GameManager.Instance.SwitchGameState(GameState.Fishing);                         
            }
            else if(GameManager.Instance.gameState == GameState.Fishing)
            {
                HookController.Instance.ShootingHook();
            }
        }
        public void SwitchUIState(UIState uistate)
        {           
            state = uistate;
            switch (state)
            {
                case UIState.Start:
                    HandleStart();
                    break;
                case UIState.Fishing:
                    HandleFishing();
                    break;
                case UIState.Slashing:
                    HandleSlash();
                    break;
               
                default:
                    break;
            }
        }  
        private void HandleStart()
        {
            ClearUI();
            uiStart.Initialize();
        }
        private void HandleFishing()
        {
            ClearUI();
            uiFishing.Initialize();           
        }
        private void HandleSlash()
        {
            ClearUI();
            uiSlash.Initialize();
        }        
        private void ClearUI()
        {
            uiStart.Clear();           
            uiFishing.Clear();
            uiSlash.Clear();
            
        }                   
    }
    public enum UIState
    {
        Start,Fishing,Slashing,BuyBooster
    }
}