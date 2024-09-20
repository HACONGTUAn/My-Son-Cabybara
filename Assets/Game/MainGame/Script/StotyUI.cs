using System;
using System.Collections;
using System.Collections.Generic;
using Capybara;
using Unity.VisualScripting;
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
            int count = Math.Min(Unlock.Count, chapters.chapter.Count);
            for(int i = 0; i < count; i++)
            {
                Unlock[i].gameObject.SetActive(!chapters.chapter[i].isUnlocked);
            }
        }
    }

}