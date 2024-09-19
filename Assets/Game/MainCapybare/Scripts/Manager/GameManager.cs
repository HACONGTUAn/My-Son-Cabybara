using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capybara
{
    public class GameManager : MonoBehaviour
    {
        public DataChapter chapter;
        void Start()
        {
            chapter.listTasks[0].tasks[1].isUnlocked = true;
        }
    }
}
