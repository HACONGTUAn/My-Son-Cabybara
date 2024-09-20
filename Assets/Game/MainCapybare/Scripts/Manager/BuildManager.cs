using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Capybara
{
    public class BuildManager : MonoBehaviour
    {
        public Transform buttonSpawn;
        public Transform chapterSpawn;
        private Follow follow;
        private void Start()
        {
            follow = GameManager.Instance.followChapter;
            LoadChapter();
        }
        private void Update() 
        {
            CheckChapter();
        }
        public void LoadChapter()
        {
            GameObject chapterObj = chapterSpawn.transform.Find(follow.listChapter.chapter[follow.chapter].chapterName)?.gameObject;
            GameObject chapterPrefab;
            if(chapterObj == null)
            {  
                chapterPrefab = Instantiate(follow.listChapter.chapter[follow.chapter].chapterPrefab, chapterSpawn);
                chapterPrefab.name = follow.listChapter.chapter[follow.chapter].chapterName;
            }
            else
            {
                chapterPrefab = chapterObj;
            }
            DataChapter dataChapter = follow.listChapter.chapter[follow.chapter].dataChapter;
            for(int i=0; i<dataChapter.listTasks.Count; i++)
            {
                ListTaskChapter listTask = dataChapter.listTasks[i];
                foreach (var task in listTask.tasks)
                {
                    GameObject taskObj = chapterPrefab.transform.Find(task.taskName)?.gameObject;
                    taskObj.SetActive(task.isUnlocked);
                    if(!task.isUnlocked && follow.task == i)
                    {
                        UIBuildButton uIBuildButton = Instantiate(Resources.Load<UIBuildButton>("UI/UiBuildButton"), buttonSpawn);
                        uIBuildButton.SpawnBuildButton(task, taskObj);
                    }
                }
            }
        }
        private void CheckChapter()
        {
            DataChapter dataChapter = follow.listChapter.chapter[follow.chapter].dataChapter;
            ListTaskChapter listTask = dataChapter.listTasks[follow.task];
            foreach (var task in listTask.tasks)
            {
                if(!task.isUnlocked)
                {
                    return;
                }
            }
            follow.task++;
            if(follow.task >= dataChapter.listTasks.Count)
            {
                follow.listChapter.chapter[follow.chapter].isUnlocked = true;
                follow.chapter++;
                follow.task = 0;
            }
            Helper.CreateCounter(0.3f, () =>
            {
                LoadChapter();
            });
        }
    }
}
