using DG.Tweening;
using UnityEngine;

namespace CapybaraJump
{
    public class Capybara_NormalFallBehavior : StateMachineBehaviour
    {
        [Header("Parameters")]
        //[SerializeField] private float fallHeight;
        [SerializeField] private float fallTime;
        [SerializeField] private Ease easeType;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float newYPos = animator.transform.position.y - InstantiateGameObject.Instance.carpetHeight*3;
            animator.transform.DOMoveY(newYPos, fallTime)
            .SetEase(this.easeType)
            .OnComplete(() =>
            {
                Debug.Log("Fall complete!");
                animator.SetTrigger("touchGround");
               
               
            });
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("Exit Fall");
            animator.transform.DOKill();
            CameraFollowController.Instance.MoveUpperOneTime();
             SpawnCarpet.Instance.UpdatePos();
            SpawnCarpet.Instance.SpawnNewCarpet();
        }
    }
}
