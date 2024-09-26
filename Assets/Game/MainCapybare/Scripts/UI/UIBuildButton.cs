using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Fishing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Capybara
{
    public class UIBuildButton : MonoBehaviour
    {
        //================================================================ SpawnButton================================================================
        public Button Build;
        public Image button;
        public Text text;
        public Image Icon;
        public List<Sprite> sprites;
        public Text Heart;
        public Text textBtn;
        public Image imageBtn;
        public List<Sprite> spriteBtn;
        public void SpawnBuildButton(TaskChapter task, GameObject taskObj)
        {
            if((int)task.type == 0)
            {
                Heart.text = "X" + task.price.ToString();
                textBtn.text = "  Do it";
            }
            else
            {
                Heart.gameObject.SetActive(false);
                textBtn.text = "  Play";
            }
            Icon.sprite = taskObj.GetComponent<SpriteRenderer>().sprite;
            text.text = "I need " + task.taskName.ToString();
            imageBtn.sprite = spriteBtn[(int)task.type];
            button.sprite = sprites[task.isCompleted ? 1:0];
            Build.onClick.AddListener(() => TaskButtonClick(task, taskObj));
        }
        public void buttonCompleted(TaskChapter task)
        {
            if((int)task.type == 0)
            {
                task.isCompleted = CapybaraMain.Manager.Instance.GetHeart()>(int)task.price;
            }
            button.sprite = sprites[task.isCompleted ? 1:0];
        }
        //==============================================Task Button================================================================
        private void TaskButtonClick(TaskChapter task, GameObject taskObj)
        {
            if(task.isCompleted)
                {
                    if(!BuildManager.Instance.isBuilding)
                    {
                        Build.interactable = false;
                        BuildManager.Instance.isBuilding = true;
                        if((int)task.type == 0)
                        {
                            UnCoinFx.Instance.PlayFx(() => UnlockTask(task, taskObj), 0, Build.transform, task.price);
                        }
                        else
                        {
                            StartCoroutine(DelayTask(task, taskObj));
                        } 
                    }   
                }
                else
                {
                    switch((int)task.type)
                    {
                        case 0://Heart

                            break;
                        case 1://Jump
                            GameManager.Instance.playgame(1);
                            break;
                        case 2://Merge
                            GameManager.Instance.playgame(0);
                            break;
                        case 3://Fishing
                            GameManager.Instance.playgame(2);
                            break;
                        default:
                            break;
                    }
                    BuildManager.Instance.task.SetActive(false);
                }
        }

        //================================================================DelayTask================================================================
        private float bottom;
        private float left;
        private float top;
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
            // Destroy(gameObject);
            BuildManager.Instance.CheckChapter();
        }
    }
}
