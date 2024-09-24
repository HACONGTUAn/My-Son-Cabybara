using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Capybara
{
    public class BuildManager : Singleton<BuildManager>
    {
        public bool isBuilding;
        public Transform buttonSpawn;
        public Transform chapterSpawn;
        public Slider chapterSlider;
        private Follow follow;
        public GameObject bottom;
        public GameObject left;
        public GameObject top;
        public GameObject task;
        private void Start()
        {
            follow = GameManager.Instance.followChapter;
            chapterSlider.value = (float)follow.task / (float)follow.listChapter.chapter[follow.chapter].dataChapter.listTasks.Count;
            CapybaraMain.Manager.Instance.SetHeart(50);
            isBuilding = false;
            LoadChapter();
        }
        private void Update() 
        {
            CheckChapter();
        }
        private void LoadChapter()
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
            float targetValue = (float)follow.task / (float)follow.listChapter.chapter[follow.chapter].dataChapter.listTasks.Count;
            StartCoroutine(SmoothSlider(targetValue));
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
        private IEnumerator SmoothSlider(float target)
        {
            float elapsedTime = 0;
            float startValue = chapterSlider.value;

            while (elapsedTime < 0.3f)
            {
                chapterSlider.value = Mathf.Lerp(startValue, target, elapsedTime / 0.3f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            chapterSlider.value = target;
        }
    }
}
