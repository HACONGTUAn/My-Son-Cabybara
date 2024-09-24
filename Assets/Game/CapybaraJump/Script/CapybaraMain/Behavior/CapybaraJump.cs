using DG.Tweening;
using UnityEngine;

namespace CapybaraJump
{
    public class CapybaraJump : StateMachineBehaviour
    {
        [SerializeField] private Ease easeType;
        [SerializeField] private float jumpTime;
        private Rigidbody2D rb;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(!GameManager.Instance.isBoost){
               
                GameObject capybara = animator.gameObject;
                rb = capybara.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * GameManager.Instance.jumpF, ForceMode2D.Impulse);

            }
          
            
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            if (rb != null)
            {
                if (rb.linearVelocity.y < 0)
                {
                    animator.SetTrigger("fall");
                }
            }
            
        }

    }
}
