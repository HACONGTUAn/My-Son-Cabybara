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
        private void LandingSuccessful(){
            
            this.StopMove();
            Debug.Log("success! next");
            animator.SetTrigger("touchGround");
            CameraFollowController.Instance.MoveUpperOneTime(1, CameraFollowController.Instance.moveTime);
            SpawnCarpet.Instance.UpdatePos();
            SpawnCarpet.Instance.SpawnNewCarpet();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == GameManager.Instance.CapybaraCarpet_LayerIndex)
            {
               
                Vector2 playerDirection = transform.position - collision.transform.GetChild(1).transform.position;
                float angle = Vector2.Angle(playerDirection, Vector2.up);
                if(angle <= GameManager.Instance.angleCollisonEnterThreshHole)
                {
                    
                    if(angle <= GameManager.Instance.perfectJumpThreshHole){
                        ScoreController.Instance.AddScore(2);
                        Debug.Log("Perfect!");
                    }
                    else {
                        ScoreController.Instance.AddScore(1);
                    }
                    ScoreController.Instance.CheckGift(this.transform.position + Vector3.up*InstantiateGameObject.Instance.carpetHeight*1.5f);
                    if(GameManager.Instance.isBoost){
                        StopMove();
                        MoveUpBooster(5);
                    }
                    else LandingSuccessful();
                   // SpawnCarpet.Instance.UpdatePos();
                }
                else
                {   
                    Debug.Log("Lose");
                    GameManager.Instance.GameOver();
                }
               // Debug.Log(angle);
            }
        }

         private void MoveUpBooster(int steps){
             Debug.Log("boóst");
            float jumpTime = 0.2f;
           // animator.SetTrigger("Jump");
            float newYPos = transform.position.y + InstantiateGameObject.Instance.carpetHeight*2;
            transform.DOMoveY(newYPos, jumpTime*0.5f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.Log("đây");
                Debug.Log(newYPos);
                 Debug.Log(InstantiateGameObject.Instance.carpetHeight*steps);
                CameraFollowController.Instance.MoveUpperOneTime(steps , jumpTime*3);
                float boostPos = transform.position.y + InstantiateGameObject.Instance.carpetHeight*steps;
                Debug.Log(boostPos);
                transform.DOMoveY(boostPos, jumpTime*3).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Debug.Log("true");
                    GameManager.Instance.isBoost = false;
                    
                });
                
                
            });
        
        } 


        private void StopMove(){
            transform.DOKill();
        }
    }

}

