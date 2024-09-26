using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CapybaraMain
{ 
    public class HomeUI : BaseUI
    {
        [SerializeField] private Text heartText;
        [SerializeField] private Text ticket;

        [SerializeField]private Button next;
        [SerializeField] private Button back;

        [SerializeField] private ScrollRect scrollSelectionGame;
        [SerializeField] private Button backHome;
        [SerializeField] private Capybara.UIDaily daiLyUp;
        [SerializeField] private GameObject Setting ;
        [SerializeField] private GameObject Build ;
        [SerializeField] private Transform test;

        private int currentIndex = 0;
        private float itemWidth;
        public GameObject currentGameObject = null;
        private GameObject saveGameObject = null;
        private bool checkGame = false;

        private void OnEnable()
        {
            heartText.text = Manager.Instance.GetHeart().ToString();
            ticket.text = Manager.Instance.GetTicket().ToString();

        }
        private void Start()
        {
            itemWidth = scrollSelectionGame.content.GetChild(0).GetComponent<RectTransform>().rect.width;
            

            scrollSelectionGame.content.GetComponent<RectTransform>().sizeDelta = new Vector2(itemWidth * 3 + 69.22f * 2, scrollSelectionGame.content.GetComponent<RectTransform>().sizeDelta.y);



        }
       public void ScrollLeft()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                UpdateScrollPosition();
            }
        }
        public void ScrollRight()
        {
            if (currentIndex < scrollSelectionGame.content.childCount - 1)
            {
                currentIndex++;
                UpdateScrollPosition();
            }
        }
        void UpdateScrollPosition()
        {
        
            float targetPosition = currentIndex * (itemWidth + 69.22f* 2);
            scrollSelectionGame.content.anchoredPosition = new Vector2(-(targetPosition ) , scrollSelectionGame.content.anchoredPosition.y);
        }
        public void OnClickPlayGame()
        {
            if (! LoadingResources.Instance.keyValuePairs.ContainsKey(currentIndex)) {
                Debug.Log("No key");
                return;
            }
            if(currentGameObject == null && checkGame == false){
                currentGameObject = Instantiate(LoadingResources.Instance.keyValuePairs[currentIndex]);
                if(currentIndex == 0)
                {
                    saveGameObject = currentGameObject;
                    checkGame = true;
                }
            }
            else
            {
                if(checkGame && currentIndex == 0){
                    currentGameObject = saveGameObject;
                    currentGameObject.SetActive(true);
                }
                else
                {
                    currentGameObject = Instantiate(LoadingResources.Instance.keyValuePairs[currentIndex]);
                }
            }
           
           
           
            
          
            this.gameObject.SetActive(false);
            backHome.gameObject.SetActive(true);
        }

        public void BackHome()
        {
            // currentGameObject.SetActive(false);
            if (currentGameObject.GetComponent<BaseID>().id == 0)
            {
                currentGameObject.SetActive(false);
                switchTypeMiniGame(currentGameObject.GetComponent<BaseID>());

            }
            else
            {
                switchTypeMiniGame(currentGameObject.GetComponent<BaseID>());
                Destroy(currentGameObject);
            }

            this.gameObject.SetActive(true);
            backHome.gameObject.SetActive(false);

            heartText.text = Manager.Instance.GetHeart().ToString();
            ticket.text = Manager.Instance.GetTicket().ToString();
        }
        void switchTypeMiniGame(BaseID obj)
        {
            if (obj is MiniGame1)
            {
                //  Debug.Log(" Mini1");
                MiniGame1 miniGame = (MiniGame1)obj;
                miniGame.UserItemInMiniGame();
            }
            else if (obj is MiniGame2)
            {
                MiniGame2 miniGame = (MiniGame2)obj;
                miniGame.UserItemInMiniGame();
                //  Debug.Log(" Mini2");
            }
            else if (obj is MiniGame3)
            {
                MiniGame3 miniGame = (MiniGame3)obj;
                miniGame.UserItemInMiniGame();
                //  Debug.Log("Mini3");
            }
        }
        // daily
        //=====================================================================================================
        public void DailyOpen()
        {
            // daiLyUp.Start();
            daiLyUp.gameObject.SetActive(true);
        }
        public void exitDaily()
        {
            daiLyUp.gameObject.SetActive(false);
        }
        // Setting 
        //========================================================================================================
        public void SettingOpen()
        {
            Setting.SetActive(true);
        }
        public void exitSetting()
        {
            Setting.SetActive(false);
        }
        // Build 
        //========================================================================================================
        public void BuildOpen()
        {
            Build.SetActive(true);
        }
        public void exitBuild()
        {
            if(!Capybara.BuildManager.Instance.isBuilding)
            {
                Build.SetActive(false);
            }
        }
    }
}
