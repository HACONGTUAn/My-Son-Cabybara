using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Merge
{
    public class UINewItemPopup : PopupUI
    {
        [SerializeField] Button closeButton;
        [SerializeField] Image Icon;
        [SerializeField] Transform glowObj;
        public override void Initialize(UIManager manager)
        {
            base.Initialize(manager);
            closeButton.onClick.AddListener(Hide);
        }

        public void SetIcon(Sprite sprite)
        {
            Icon.sprite = sprite;
            glowObj.DORotate(new Vector3(0, 0, 180), 2f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear).SetId(this);
        }
        public override void Hide()
        {
            DOTween.Kill(this);
            base.Hide();
        }
    }

}