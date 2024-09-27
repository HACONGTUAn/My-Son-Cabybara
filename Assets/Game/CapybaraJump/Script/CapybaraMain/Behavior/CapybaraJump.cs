using DG.Tweening;
using UnityEngine;

namespace CapybaraJump
{
    public class CapybaraJump : StateMachineBehaviour
    {
        [SerializeField] private Ease easeType;
        [SerializeField] private float jumpTime;
        private Rigidbody2D rb;
        private bool isFall = false;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
           if(!GameManager.Instance.isBoost && !GameManager.Instance.gameOver)
            {
                isFall = false;
                GameObject capybara = animator.transform.parent.gameObject;
                Debug.Log(capybara.name);
                rb = capybara.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * GameManager.Instance.jumpF, ForceMode2D.Impulse);

            }
            
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            if (rb != null)
            {
                if (rb.velocity.y < 0 && !isFall)
                {
                    isFall = true;
                   
                    //CapybaraMain.Instance.GetComponent<Rigidbody2D>().gravityScale = GameManager.Instance.gravityScale;
                    animator.SetTrigger("fall");
                }
            }
            
        }

    }
}
