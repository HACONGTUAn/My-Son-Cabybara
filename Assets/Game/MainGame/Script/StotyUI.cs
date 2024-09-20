using System.Collections;
using System.Collections.Generic;
using Capybara;
using UnityEngine;

namespace CapybaraMain
{ 
    public class StotyUI : BaseUI
    {
        public List<GameObject> Unlock;
        public DataListChapter chapters;
        private void Update()
        {
            UnlockChapter();
        }
        private void UnlockChapter()
        {
            for(int i = 0; i < chapters.chapter.Count; i++)
            {
                Unlock[i].gameObject.SetActive(!chapters.chapter[i].isUnlocked);
            }
        }
    }

}