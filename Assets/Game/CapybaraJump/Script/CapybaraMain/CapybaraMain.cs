using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CapybaraJump
{
    public class CapybaraMain : MonoBehaviour
    {

        private Animator animator;

        
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == GameManager.Instance.CapybaraCarpet_LayerIndex)
            {
               
                Vector2 playerDirection = transform.position - collision.transform.GetChild(1).transform.position;
                float angle = Vector2.Angle(playerDirection, Vector2.up);
                if(angle <= GameManager.Instance.angleCollisonEnterThreshHole)
                {
                    this.StopMove();
                    Debug.Log("success! next");
                    animator.SetTrigger("touchGround");
                   // SpawnCarpet.Instance.UpdatePos();
                }
                else
                {   
                    Debug.Log("Lose");
                    GameManager.Instance.GameOver();
                }
                Debug.Log(angle);
            }
        }


        private void StopMove(){
            transform.DOKill();
        }
    }

}

