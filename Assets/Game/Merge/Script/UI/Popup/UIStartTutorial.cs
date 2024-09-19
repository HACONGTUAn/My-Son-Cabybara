using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Merge
{
    public class UIStartTutorial : PopupUI
    {
        [SerializeField] Transform hand;
        [SerializeField] GameObject tutorialPanel;

        public override void Initialize(UIManager manager)
        {
            base.Initialize(manager);
            StartHandMovement();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                gameObject.SetActive(false);
            }
        }

        private void AnimatedUI(Transform popup)
        {
            popup.localScale = Vector3.zero;
            popup.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetUpdate(true);
        }

        private void StartHandMovement()
        {
            tutorialPanel.SetActive(true);
            hand.DOLocalMoveX(-400, 2.5f).SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo); 
        }
    }

}