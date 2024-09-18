using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;
namespace Merge
{
    public class UINewFruitPopup : PopupUI
    {
        [SerializeField] Button closeButton;
        [SerializeField] Transform holder;
        [SerializeField] Transform glowObj;
        [SerializeField] SkeletonGraphic skeletonGraphic;
        public override void Initialize(UIManager manager)
        {
            base.Initialize(manager);
            closeButton.onClick.AddListener(Hide);
        }

        public void SetIcon(Fruit f)
        {
            var fruit = Instantiate(f, holder);
            fruit.Initialize();
            fruit.transform.localScale = Vector3.one;
            fruit.transform.localPosition = Vector3.zero;
            fruit.DisablePhysic();
            var r = fruit.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < r.Length; i++)
            {
                r[i].gameObject.layer = LayerMask.NameToLayer("UI");
            }
            skeletonGraphic.AnimationState.AddAnimation(0, "Show", false, 0.1f);
            skeletonGraphic.AnimationState.AddAnimation(0, "Idle", true, -1);
            glowObj.DORotate(new Vector3(0, 0, 180), 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear).SetId(this);
        }
        public override void Hide()
        {
            // AdsHelperWrapper.ShowFull("new_fruit");
            DOTween.Kill(this);
            base.Hide();
        }
    }

}