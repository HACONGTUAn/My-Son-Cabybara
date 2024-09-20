using UnityEngine;

namespace CapybaraJump
{
    public class CapybaraIdle : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
           /*  Debug.Log("idle"); */
        }


        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //if (Input.GetKeyDown(KeyCode.Mouse0) && !GameManager.Instance.isBoost)
            //{
            //    animator.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            //    animator.SetTrigger("Jump");

            //}
            
            
        }
       
        
    }
}