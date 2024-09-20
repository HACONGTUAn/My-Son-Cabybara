using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

namespace CapybaraJump
{
    public class CapybaraMain : MonoBehaviour
    {

        public static CapybaraMain Instance;
        private Animator animator;
        public bool isMoving = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();

        }

        // Update is called once per frame
        void Update()
        {
           
        }
        public void LandingSuccessful()
        {
            if (!GameManager.Instance.isShield)
            {
                this.StopMove();
            }
            Debug.Log("success! next");
            animator.SetTrigger("touchGround");
            CameraFollowController.Instance.MoveUpperOneTime(1, CameraFollowController.Instance.moveTime);
            SpawnCarpet.Instance.UpdatePos();
            SpawnCarpet.Instance.SpawnNewCarpet(2f);
        }

        /*   private void OnTriggerEnter2D(Collider2D collision)
           {
               if (collision.gameObject.layer == GameManager.Instance.CapybaraCarpet_LayerIndex)
               {

                   Vector2 playerDirection = transform.position - collision.transform.GetChild(1).transform.position;
                   float angle = Vector2.Angle(playerDirection, Vector2.up);
                   if(angle <= GameManager.Instance.angleCollisonEnterThreshHole)
                   {

                       if(angle <= GameManager.Instance.perfectJumpThreshHole){
                           ScoreController.Instance.AddScore(1);// fix
                           Debug.Log("Perfect!");
                       }
                       else {
                           ScoreController.Instance.AddScore(1);
                       }
                       ScoreController.Instance.CheckGift(this.transform.position + Vector3.up*InstantiateGameObject.Instance.carpetHeight*1.5f);
                       if(GameManager.Instance.isBoost){

                           StopMove();
                           MoveUpBooster(10);
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
           }*/

        public void MoveUpBooster(int steps)
        {

            float jumpTime = GameManager.Instance.jumpTime;

            float newYPos = transform.position.y + InstantiateGameObject.Instance.carpetHeight * 2;
            transform.DOMoveY(newYPos, jumpTime * 0.5f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {

                Debug.Log(newYPos);
                Debug.Log(InstantiateGameObject.Instance.carpetHeight * steps);
                CameraFollowController.Instance.MoveUpperOneTime(steps, jumpTime * 3);
                float boostPos = transform.position.y + InstantiateGameObject.Instance.carpetHeight * (steps);
                Debug.Log(boostPos);
                transform.DOMoveY(boostPos, jumpTime * 3).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    transform.DOKill();

                    GameManager.Instance.isBoost = false;
                    float newYPos = transform.position.y - InstantiateGameObject.Instance.carpetHeight * 3;
                    Debug.Log("đây" + newYPos);
                    float fallTime = GameManager.Instance.fallTime;
                    transform.DOMoveY(newYPos, fallTime * 1.5f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {

                        Debug.Log("Fall complete!");
                        animator.SetTrigger("touchGround");
                    });


                });
                StartCoroutine(Spawn(steps));


                IEnumerator Spawn(int steps)
                {
                    int index = 1;
                    while (index <= steps)
                    {

                        SpawnCarpet.Instance.UpdatePos();
                        SpawnCarpet.Instance.SpawnNewCarpet(0.5f);
                        index++;


                    }
                    yield return null;
                }
            });

        }


        public void StopMove()
        {
            transform.DOKill();
            animator.SetTrigger("touchGround");
           
            animator.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            animator.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

        public void ClickToJump()
        {
            Debug.Log("false");
            if(!GameManager.Instance.isBoost)
            {
                animator.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                animator.SetTrigger("Jump");


            }

        }
    }

}

