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
        public Animator animator;
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


           
            animator.SetTrigger("touchGround");
            //if (!ScoreController.Instance.shield.activeSelf)
            //{
            //    Debug.Log("not move");
            //    GameManager.Instance.isShield = false;
            //}
            if (!GameManager.Instance.isShield)
            {
                this.StopMove();
                Debug.Log("first");

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
                if (ScoreController.Instance.shield.activeSelf)
                {
                    Debug.Log("third");
                    GameManager.Instance.oldCarpet = carpet;
                    CameraFollowController.Instance.MoveUpperOneTime(1, CameraFollowController.Instance.moveTime);
                    SpawnCarpet.Instance.UpdatePos();
                    SpawnCarpet.Instance.SpawnNewCarpet(2f);

                }
                else
                {
                    Debug.Log("second");
                    GameManager.Instance.isShield = false;
                    SpawnCarpet.Instance.SpawnNewCarpet(2f);
                    GameManager.Instance.isJustShield = false;

                }
               

            }

           


        }

        public void MoveUpBooster(int steps)
        {
            GameManager.Instance.oldCarpet = null;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            float jumpTime = GameManager.Instance.jumpTime;

            float newYPos = transform.position.y + InstantiateGameObject.Instance.carpetHeight * 2;
            transform.DOMoveY(newYPos, jumpTime * 1f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {

                Debug.Log(newYPos);
                Debug.Log(InstantiateGameObject.Instance.carpetHeight * steps);
                CameraFollowController.Instance.MoveUpperOneTime(steps, jumpTime * 4);
                float boostPos = transform.position.y + InstantiateGameObject.Instance.carpetHeight * (steps+1);
                Debug.Log(boostPos);
                transform.DOMoveY(boostPos, jumpTime * 4).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    transform.DOKill();
                    GameManager.Instance.isBoost = false;
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    GetComponent<Rigidbody2D>().gravityScale = GameManager.Instance.gravityScale;
            

                });
                StartCoroutine(Spawn(steps));


                IEnumerator Spawn(int steps)
                
                {
                   
                  
                    for(int index = 0; index < steps; index++) 
                    {

                        SpawnCarpet.Instance.UpdatePos();
                        SpawnCarpet.Instance.SpawnNewCarpet(0.25f);
                        yield return new WaitForSeconds(0.12f);
                        ScoreController.Instance.AddScore(1);

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
        //public void GameOver()
        //{
        //    GameManager.Instance.GameOver();
        //    StopMove();
        //}

        public void ClickToJump()
        {
            
            if(!GameManager.Instance.isBoost && animator.GetComponent<Rigidbody2D>().velocity == Vector2.zero )
            {
                animator.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                GetComponent<Rigidbody2D>().gravityScale = GameManager.Instance.gravityScale;
                animator.SetTrigger("Jump");


            }

        }
        public void Die(int dimensity)
        {
            float xPos = -2* dimensity;
            transform.DOMoveX(xPos, 0.3f)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    StartCoroutine(wait());
                    IEnumerator wait()
                    {
                        yield return new WaitForSeconds(0.5f);
                        GameManager.Instance.GameOver();
                        StopMove();
                    }
                   
                });

        }
    }

}

