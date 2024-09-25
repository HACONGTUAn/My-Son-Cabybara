using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capybara
{
    public class GameManager : Singleton<GameManager>
    {
        public Follow followChapter;
        public CapybaraMain.HomeUI homeUI;
        
        public void exit(){
            homeUI.BackHome();
        }
    }
}
