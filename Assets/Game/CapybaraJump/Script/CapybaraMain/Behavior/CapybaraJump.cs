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
            if(!GameManager.Instance.isBoost){
                float newYPos = animator.transform.position.y + InstantiateGameObject.Instance.carpetHeight*3;
            
                animator.transform.DOMoveY(newYPos, jumpTime).SetEase(this.easeType)
                .OnComplete(() =>
                {
                    /* if(!GameManager.Instance.isBoost){ */
                        Debug.Log("this");
                        animator.SetTrigger("fall");

                    //}
                    
                });
            }
          
            
        }
        
    }
}
