using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CapybaraMain
{ 
    public class HomeUI : BaseUI
    {
        
        public Button next;
        public Button back;
        public ScrollRect scrollSelectionGame;
        public Button backHome;

        private int currentIndex = 0;
        private float itemWidth;
        private GameObject currentGameObject;
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
            currentGameObject = Instantiate(LoadingResources.Instance.keyValuePairs[currentIndex]);
            this.gameObject.SetActive(false);
            backHome.gameObject.SetActive(true);
        }

        public void BackHome()
        {
            Destroy(currentGameObject);
            this.gameObject.SetActive(true);
            backHome.gameObject.SetActive(false);
        }
    }
}
