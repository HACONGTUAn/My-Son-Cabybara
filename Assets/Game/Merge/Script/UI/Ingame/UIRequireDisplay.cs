using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Merge
{
    public class UIRequireDisplay : MonoBehaviour
    {
        public Image iconMission;
        public Text requireAmount;
        public GameObject completeMark;
        private int previousAmount;
        public ParticleSystem completionParticles;
        private Vector3 originalScale;
        private bool animated;
        private ClassicMode classicMode;
        public MissionRequire missionRequire;
        public Transform glowObj;

        private void Awake()
        {
            originalScale = Vector3.one;
        }

        public void SetInfo(MissionRequire missionRequire)
        {
            DOTween.Kill(this);
            SetActive(true);
            this.missionRequire = missionRequire;
            requireAmount.gameObject.SetActive(true);
            var itemObjectContainer = DataManager.Instance.itemObjectContainer;
            ItemObjectSO item = itemObjectContainer.GetItemObject(missionRequire.itemID, missionRequire.itemType);
            iconMission.sprite = item.icon;
            if (GameManager.Instance.currentMode)
            {
                classicMode = GameManager.Instance.currentMode.GetComponent<ClassicMode>();
            }
            //if (missionRequire.itemType == ItemType.Fruit && ClassicMode.onFruitFlying) { return; }
            if (previousAmount != missionRequire.requireAmount)
            {
                if (missionRequire.requireAmount > 0)
                {
                    AnimateScaleChange();
                    previousAmount = missionRequire.requireAmount;
                }
            }
            if (missionRequire.requireAmount > 0)
            {
                completeMark.SetActive(false);
                requireAmount.text = missionRequire.requireAmount.ToString();
                animated = false;
            }
            else
            {
                completeMark.SetActive(true);
                requireAmount.gameObject.SetActive(false);
                if (!animated)
                {
                    PlayCompletionParticles();
                    PlayCompletionAnimation();
                }
                animated = true;
            }
        }

        public void SetActive(bool status)
        {
            gameObject.SetActive(status);
        }

        private void AnimateScaleChange()
        {
            transform.DOScale(originalScale * 1.5f, 0.2f).OnComplete(() =>
            {
                transform.DOScale(originalScale, 0.2f);
            });
        }

        private void PlayCompletionParticles()
        {
            if (completionParticles != null)
            {
                ParticleSystem completion = Instantiate(completionParticles, transform.position, Quaternion.identity, transform.parent);
                completion.transform.localScale = Vector3.one * 200f;
                int uiLayer = LayerMask.NameToLayer("UI");
                if (uiLayer == -1)
                {
                    Debug.LogError("Layer 'UI' không tồn tại.");
                }
                else
                {
                    completion.gameObject.layer = uiLayer;
                    foreach (Transform child in completion.transform)
                    {
                        child.gameObject.layer = uiLayer;
                    }
                }
                completion.Play();
            }
        }

        private void PlayCompletionAnimation()
        {
            glowObj.gameObject.SetActive(true);
            glowObj.DORotate(new Vector3(0, 0, 180), 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear).SetId(this).SetUpdate(true);
            transform.DOScale(originalScale * 1.5f, 1.2f).OnComplete(() =>
            {
                transform.DOScale(originalScale, 0.2f);
                glowObj.gameObject.SetActive(false);
            });
        }
    }
}