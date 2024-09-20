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
        public Rigidbody2D rb;

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
            rb = GetComponent<Rigidbody2D>();

        }

        // Update is called once per frame
        void Update()
        {
           
        }
        public void LandingSuccessful(GameObject carpet)
        {
            
            this.StopMove();
            
            Debug.Log("success! next");
            animator.SetTrigger("touchGround");
            if (!ScoreController.Instance.shield.activeSelf)
            {
                Debug.Log("not move");
                GameManager.Instance.isShield = false;
            }
            if (!GameManager.Instance.isJustShield)
            {
                if (carpet == GameManager.Instance.oldCarpet)
                {
                  //  SpawnCarpet.Instance.SpawnNewCarpet(2f);
                    return;
                }
                GameManager.Instance.oldCarpet = carpet;


                CameraFollowController.Instance.MoveUpperOneTime(1, CameraFollowController.Instance.moveTime);
                SpawnCarpet.Instance.UpdatePos();
                SpawnCarpet.Instance.SpawnNewCarpet(2f);
            }
            else
            {
                Debug.Log("jusst");
                SpawnCarpet.Instance.SpawnNewCarpet(2f);
                GameManager.Instance.isJustShield = false;

            }

           


        }

        public void MoveUpBooster(int steps)
        {
            GameManager.Instance.oldCarpet = null;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            float jumpTime = GameManager.Instance.jumpTime;

            float newYPos = transform.position.y + InstantiateGameObject.Instance.carpetHeight * 2;
            transform.DOMoveY(newYPos, jumpTime * 0.5f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {

                Debug.Log(newYPos);
                Debug.Log(InstantiateGameObject.Instance.carpetHeight * steps);
                CameraFollowController.Instance.MoveUpperOneTime(steps, jumpTime * 3);
                float boostPos = transform.position.y + InstantiateGameObject.Instance.carpetHeight * (steps+1);
                Debug.Log(boostPos);
                transform.DOMoveY(boostPos, jumpTime * 3).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    transform.DOKill();

                    GameManager.Instance.isBoost = false;
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    //float newYPos = transform.position.y - InstantiateGameObject.Instance.carpetHeight * 3;
                    //Debug.Log("đây" + newYPos);
                    //float fallTime = GameManager.Instance.fallTime;
                    //transform.DOMoveY(newYPos, fallTime * 1.5f)
                    //.SetEase(Ease.Linear)
                    //.OnComplete(() =>
                    //{

                    //    Debug.Log("Fall complete!");
                    //    animator.SetTrigger("touchGround");
                    //});


                });
                StartCoroutine(Spawn(steps));


                IEnumerator Spawn(int steps)
                
                {
                    yield return new WaitForSeconds(0.6f);
                    int index = 1;
                    while (index <= steps)
                    {

                        SpawnCarpet.Instance.UpdatePos();
                        SpawnCarpet.Instance.SpawnNewCarpet(0.35f);
                        ScoreController.Instance.AddScore(1);
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
            if(!GameManager.Instance.isBoost && animator.GetComponent<Rigidbody2D>().velocity == Vector2.zero )
            {
                animator.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                animator.SetTrigger("Jump");


            }

        }
    }

}

