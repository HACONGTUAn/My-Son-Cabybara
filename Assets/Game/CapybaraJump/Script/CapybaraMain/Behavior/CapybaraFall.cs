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
            Debug.Log("fallllll");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
           // Debug.Log("Exit Fall");
           // animator.transform.DOKill();
          
        }
    }
}
