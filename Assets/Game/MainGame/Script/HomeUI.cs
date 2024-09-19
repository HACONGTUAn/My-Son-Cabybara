using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CapybaraMain
{ 
    public class HomeUI : BaseUI
    {
        
        [SerializeField]private Button next;
        [SerializeField] private Button back;
        [SerializeField] private ScrollRect scrollSelectionGame;
        [SerializeField] private Button backHome;
        [SerializeField] private GameObject daiLyUp;
        [SerializeField] private GameObject Setting ;
        [SerializeField] private Transform test;

        private int currentIndex = 0;
        private float itemWidth;
        private GameObject currentGameObject = null;
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
            
            currentGameObject = Instantiate(LoadingResources.Instance.keyValuePairs[currentIndex], test);
            
          
            this.gameObject.SetActive(false);
            backHome.gameObject.SetActive(true);
        }

        public void BackHome()
        {
           // currentGameObject.SetActive(false);
            Destroy(currentGameObject);
            this.gameObject.SetActive(true);
            backHome.gameObject.SetActive(false);
        }

        public void DailyOpen()
        {
            daiLyUp.SetActive(true);
        }
        public void exitDaily()
        {
            daiLyUp.SetActive(false);
        }

        public void SettingOpen()
        {
            Setting.SetActive(true);
        }
        public void exitSetting()
        {
            Setting.SetActive(false);
        }
    }
}
