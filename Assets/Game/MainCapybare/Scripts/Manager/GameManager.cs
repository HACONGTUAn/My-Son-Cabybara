using System.Collections;
using System.Collections.Generic;
using CapybaraMain;
using UnityEngine;

namespace Capybara
{
    public class GameManager : Singleton<GameManager>
    {
        public Follow followChapter;
        public CapybaraMain.HomeUI homeUI;

        private GameObject saveGameObject = null;
        private bool checkGame = false;
        public void exit(){
            homeUI.BackHome();
        }
        public void playgame(int currentIndex)
        {
             if (! CapybaraMain.LoadingResources.Instance.keyValuePairs.ContainsKey(currentIndex)) {
                Debug.Log("No key");
                return;
            }
            if(homeUI.currentGameObject == null && checkGame == false){
                homeUI.currentGameObject = Instantiate(CapybaraMain.LoadingResources.Instance.keyValuePairs[currentIndex]);
                if(currentIndex == 0)
                {
                    saveGameObject = homeUI.currentGameObject;
                    checkGame = true;
                }
            }
            else
            {
                if(checkGame && currentIndex == 0){
                    homeUI.currentGameObject = saveGameObject;
                    homeUI.currentGameObject.SetActive(true);
                }
                else
                {
                    homeUI.currentGameObject = Instantiate(CapybaraMain.LoadingResources.Instance.keyValuePairs[currentIndex]);
                }
            }
            homeUI.gameObject.SetActive(false);
        }
    }
}
