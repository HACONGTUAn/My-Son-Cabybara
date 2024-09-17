using DG.Tweening;
using UnityEngine;

namespace CapybaraJump
{
    public class CapybaraJump : StateMachineBehaviour
    {
        [SerializeField] private Ease easeType;
        [SerializeField] private float jumpTime;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float newYPos = animator.transform.position.y + InstantiateGameObject.Instance.carpetHeight*3;
            
            animator.transform.DOMoveY(newYPos, jumpTime).SetEase(this.easeType)
            .OnComplete(() =>
            {
                Debug.Log("Movement complete!");
                animator.SetTrigger("fall");
            });;
        }

        /* public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.transform.DOKill();
        } */
    }
}
