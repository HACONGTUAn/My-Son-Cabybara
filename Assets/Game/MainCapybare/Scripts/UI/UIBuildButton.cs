using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Capybara
{
    public class UIBuildButton : MonoBehaviour
    {
        public Image Icon;
        public Text Heart;
        public Button Build;
        public void SpawnBuildButton(TaskChapter task, GameObject taskObj)
        {
            Icon.sprite = taskObj.GetComponent<SpriteRenderer>().sprite;
            Heart.text = "X" + task.price.ToString();
            Build.onClick.AddListener(() => TaskButtonClick(task, taskObj));
        }
        private void TaskButtonClick(TaskChapter task, GameObject taskObj)
        {
            task.isUnlocked = true;
            gameObject.SetActive(!task.isUnlocked);
            Destroy(gameObject);
            taskObj.SetActive(task.isUnlocked);
        }
    }
}
