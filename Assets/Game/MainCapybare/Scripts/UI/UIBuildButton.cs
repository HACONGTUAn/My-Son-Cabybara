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
            if(CapybaraMain.Manager.Instance.GetHeart() >= task.price && !BuildManager.Instance.isBuilding)
            {
                Build.interactable = false;
                BuildManager.Instance.isBuilding = true;
                UnCoinFx.Instance.PlayFx(() => UnlockTask(task, taskObj), 0, Build.transform, task.price);
            }
        }
        private void UnlockTask(TaskChapter task, GameObject taskObj)
        {
            CapybaraMain.Manager.Instance.SetHeart(CapybaraMain.Manager.Instance.GetHeart() - 1);
            CapybaraMain.HearTicker.Instance.ChangeValue();
            StartCoroutine(DelayTask(task, taskObj));
        }
        IEnumerator DelayTask (TaskChapter task, GameObject taskObj)
        {
            yield return new WaitForSeconds(0.5f);
            task.isUnlocked = true;
            gameObject.SetActive(!task.isUnlocked);
            Destroy(gameObject);
            taskObj.SetActive(task.isUnlocked);
            BuildManager.Instance.isBuilding = false;
        }

    }
}
