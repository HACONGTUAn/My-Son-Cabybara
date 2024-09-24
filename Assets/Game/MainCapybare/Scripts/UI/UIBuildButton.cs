using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Capybara
{
    public class UIBuildButton : MonoBehaviour
    {
        //
        public Image Icon;
        public Text Heart;
        public Button Build;
        private float bottom;
        private float left;
        private float top;
        public void SpawnBuildButton(TaskChapter task, GameObject taskObj)
        {
            Icon.sprite = taskObj.GetComponent<SpriteRenderer>().sprite;
            Heart.text = "X" + task.price.ToString();
            Build.onClick.AddListener(() => TaskButtonClick(task, taskObj));
        }
        private void TaskButtonClick(TaskChapter task, GameObject taskObj)
        {
            switch ((int)task.type)
            {
                case 0://Heart
                    HeartButton(task, taskObj);
                    break;
                case 1://Jump
                    JumpButton(task, taskObj);
                    break;
                case 2://Merge
                    MergeButton(task, taskObj);
                    break;
                case 3://Fishing
                    FishingButton(task, taskObj);
                    break;
                default:
                    break;
            }
            
        }
        private void HeartButton(TaskChapter task, GameObject taskObj)
        {
            if(CapybaraMain.Manager.Instance.GetHeart() >= task.price && !BuildManager.Instance.isBuilding)
            {
                Build.interactable = false;
                BuildManager.Instance.isBuilding = true;
                UnCoinFx.Instance.PlayFx(() => UnlockTask(task, taskObj), 0, Build.transform, task.price);
            }
        }
        private void JumpButton(TaskChapter task, GameObject taskObj)
        {
            
        }
        private void MergeButton(TaskChapter task, GameObject taskObj)
        {
            
        }
        private void FishingButton(TaskChapter task, GameObject taskObj)
        {
            
        }
        private void UnlockTask(TaskChapter task, GameObject taskObj)
        {
            CapybaraMain.Manager.Instance.SetHeart(CapybaraMain.Manager.Instance.GetHeart() - 1);
            CapybaraMain.HearTicker.Instance.ChangeValue();
            StartCoroutine(DelayTask(task, taskObj));
        }
        IEnumerator DelayTask (TaskChapter task, GameObject taskObj)
        {
            bottom = BuildManager.Instance.bottom.transform.position.y;
            left = BuildManager.Instance.left.transform.position.x;
            top = BuildManager.Instance.top.transform.position.y;
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(FirstAction());
            yield return StartCoroutine(SecondAction(task, taskObj));
            yield return StartCoroutine(ThirdAction(task));
        }
        IEnumerator FirstAction()
        {
            BuildManager.Instance.task.transform.DOScale(new Vector3(0, 0, 0), 1f);
            BuildManager.Instance.bottom.transform.DOMoveY(bottom - 2048, 1f);
            BuildManager.Instance.left.transform.DOMoveX(left - 2048, 1f);
            BuildManager.Instance.top.transform.DOMoveY(top + 2048, 1f);
            yield return new WaitForSeconds(2f); 
        }
        IEnumerator SecondAction(TaskChapter task, GameObject taskObj)
        {
            task.isUnlocked = true;
            taskObj.SetActive(task.isUnlocked);
            yield return new WaitForSeconds(2f); 
        }
        IEnumerator ThirdAction(TaskChapter task)
        {
            BuildManager.Instance.task.transform.DOScale(new Vector3(1, 1, 1), 1f);
            BuildManager.Instance.bottom.transform.DOMoveY(bottom, 1f);
            BuildManager.Instance.left.transform.DOMoveX(left, 1f);
            BuildManager.Instance.top.transform.DOMoveY(top, 1f);
            yield return new WaitForSeconds(1.5f); 
            BuildManager.Instance.isBuilding = false;
            gameObject.SetActive(!task.isUnlocked);
            Destroy(gameObject);
            BuildManager.Instance.CheckChapter();
        }

    }
}
