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
        public bool gameOverCalled = false;
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
            //animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();

        }

        // Update is called once per frame
        void Update()
        {
           
        }
        public void LandingSuccessful(GameObject carpet)
        {


            isMoving = false;
            animator.SetTrigger("touchGround");

            if (!GameManager.Instance.isShield)
            {
                this.StopMove();
                Debug.Log("first");

                if (carpet == GameManager.Instance.oldCarpet)
                {
                    return;
                }
                GameManager.Instance.oldCarpet = carpet;
                CameraFollowController.Instance.MoveUpperOneTime(1, CameraFollowController.Instance.moveTime);
                SpawnCarpet.Instance.UpdatePos();
                SpawnCarpet.Instance.SpawnNewCarpet(2f);
            }
            else
            {
                if (carpet == GameManager.Instance.oldCarpet)
                {
                    return;
                }
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

                }
               

            }

           


        }

        public void MoveUpBooster(int steps)
        {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GameManager.Instance.oldCarpet = null;
            float jumpTime = GameManager.Instance.jumpTime;

            float newYPos = transform.position.y + InstantiateGameObject.Instance.carpetHeight * 2;
            transform.DOMoveY(newYPos, jumpTime * 1f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                StartCoroutine(boost());

                IEnumerator boost()
                {
                    transform.GetChild(1).gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                    Debug.Log(newYPos);
                    Debug.Log(InstantiateGameObject.Instance.carpetHeight * steps);
                    CameraFollowController.Instance.MoveUpperOneTime(steps, jumpTime * 4);
                    float boostPos = transform.position.y + InstantiateGameObject.Instance.carpetHeight * (steps + 1);
                    Debug.Log(boostPos);
                    transform.DOMoveY(boostPos, jumpTime * 4).SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {

                       
                        transform.DOKill();
                        GameManager.Instance.isBoost = false;
                        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        GetComponent<Rigidbody2D>().gravityScale = GameManager.Instance.gravityScale;

                       
                        transform.GetChild(1).gameObject.SetActive(false);

                    });
                    StartCoroutine(Spawn(steps));


                    IEnumerator Spawn(int steps)

                    {


                        for (int index = 0; index < steps; index++)
                        {

                            SpawnCarpet.Instance.UpdatePos();
                            SpawnCarpet.Instance.SpawnNewCarpet(0.25f);
                            yield return new WaitForSeconds(0.12f);
                            ScoreController.Instance.AddScore(1);

                        }
                        yield return null;
                    }
                }
               

               
            });

        }


        public void StopMove()
        {
            transform.DOKill();
            //animator.SetTrigger("touchGround");
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

        public void ClickToJump()
        {
            
            if(!GameManager.Instance.isBoost  && !GameManager.Instance.gameOver && !isMoving)
            {
                isMoving = true;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
               
                animator.SetTrigger("Jump");


            }

        }
        public void Die(int dimensity)
        {
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            transform.GetComponent<BoxCollider2D>().isTrigger = true;
            rb.gravityScale = 0.3f;
            transform.DOKill();
            Debug.Log("die");
            float xPos = -1.5f * dimensity;
            transform.DOMoveX(xPos, 0.2f)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    StartCoroutine(wait());
                    IEnumerator wait()
                    {
                        yield return new WaitForSeconds(0.5f);
                        if (!gameOverCalled) 
                        {
                            gameOverCalled = true; 
                            StopMove();
                            GameManager.Instance.GameOver();
                        }
                    }
                   
                });
            StartCoroutine(GameOverAfterDelay(0.8f));
        }

        private IEnumerator GameOverAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (!gameOverCalled) 
            {
                gameOverCalled = true; 
                StopMove();
                GameManager.Instance.GameOver();
            }
        }
    }

}

