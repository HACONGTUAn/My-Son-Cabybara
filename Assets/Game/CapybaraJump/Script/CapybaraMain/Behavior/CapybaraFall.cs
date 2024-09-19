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
            //float newYPos = animator.transform.position.y - InstantiateGameObject.Instance.carpetHeight*4;
            //fallTime = GameManager.Instance.fallTime;
            //animator.transform.DOMoveY(newYPos, fallTime*1.5f)

            //.SetEase(this.easeType)
            //.OnComplete(() =>
            //{
            //    Debug.Log("Fall complete!");
            //    animator.SetTrigger("touchGround");
            //});
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
           // Debug.Log("Exit Fall");
            animator.transform.DOKill();
          
        }
    }
}
