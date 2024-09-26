using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Capybara
{
    public class BuildManager : Singleton<BuildManager>
    {
        //
        public bool isBuilding;
        public Transform buttonSpawn;
        public Transform chapterSpawn;
        public Slider inchapterSlider;
        public Slider outchapterSlider;
        public Text inChapterText;
        public Text outChapterText;
        public Text inTaskText;
        public Text outTaskText;
        private Follow follow;
        public GameObject bottom;
        public GameObject left;
        public GameObject top;
        public GameObject task;
        [HideInInspector] public Dictionary<UIBuildButton,TaskChapter> uIBuilds = new Dictionary<UIBuildButton,TaskChapter>();
        private void Start()
        {
            follow = GameManager.Instance.followChapter;
            inchapterSlider.value = (float)follow.task / (float)follow.listChapter.chapter[follow.chapter].dataChapter.listTasks.Count;
            outchapterSlider.value = (float)follow.task / (float)follow.listChapter.chapter[follow.chapter].dataChapter.listTasks.Count;
            inChapterText.text = "Chapter " +  (follow.chapter+1).ToString();
            outChapterText.text = "Chap " + (follow.chapter+1).ToString(); 
            inTaskText.text = follow.task.ToString() + "/" + follow.listChapter.chapter[follow.chapter].dataChapter.listTasks.Count.ToString();
            outTaskText.text = follow.task.ToString() + "/" + follow.listChapter.chapter[follow.chapter].dataChapter.listTasks.Count.ToString();
            CapybaraMain.Manager.Instance.SetHeart(0);
            isBuilding = false;
            LoadChapter();
        }
        private void Update() 
        {
            foreach(var button in uIBuilds)
            {
                button.Key.buttonCompleted(button.Value);
            }

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
                        uIBuilds.Add(uIBuildButton,task);
                        uIBuildButton.SpawnBuildButton(task, taskObj);
                    }
                }
            }
        }
        public void CheckChapter()
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
                inChapterText.text = "Chapter " +  (follow.chapter+1).ToString();
                outChapterText.text = "Chap " + (follow.chapter+1).ToString();
                follow.task = 0;
            }
            Helper.CreateCounter(0.3f, () =>
            {
                uIBuilds.Clear();
                LoadChapter();
            });
        }
        private IEnumerator SmoothSlider(float target)
        {
            float elapsedTime = 0;
            float startValue = inchapterSlider.value;

            while (elapsedTime < 0.3f)
            {
                inchapterSlider.value = Mathf.Lerp(startValue, target, elapsedTime / 0.3f);
                outchapterSlider.value = Mathf.Lerp(startValue, target, elapsedTime / 0.3f);;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            inchapterSlider.value = target;
            outchapterSlider.value = target;
            inTaskText.text = follow.task.ToString() + "/" + follow.listChapter.chapter[follow.chapter].dataChapter.listTasks.Count.ToString();
            outTaskText.text = follow.task.ToString() + "/" + follow.listChapter.chapter[follow.chapter].dataChapter.listTasks.Count.ToString();
        }
    }
}
