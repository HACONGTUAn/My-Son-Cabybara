using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

namespace CapybaraJump
{

    public class CapybaraCarpet : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private Ease easeType;
        [SerializeField] private float moveTime;
        public bool isMoving = false;
        private Vector3 startPosition;
        public bool isRight;

        [SerializeField] private Animator animator;
        
      

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        

        private void OnCollisionEnter2D(Collision2D collision)
        {
          
            if (GameManager.Instance.isBoost)
            {
                MoveBack(0.5f);
                return;
            }
            if (this.gameObject == GameManager.Instance.floorCarpet)
            {
                CapybaraMain.Instance.isMoving = false;
                CapybaraMain.Instance.animator.SetTrigger("touchGround");
                CapybaraMain.Instance.StopMove();
                return;
            }
            if (GameManager.Instance.gameOver)
            {
                return;
            }
           
        
             if (collision.gameObject.layer == GameManager.Instance.CapybaraMain_LayerIndex)
            {
               
               
                Vector2 playerDirection = collision.transform.position - transform.GetChild(1).transform.position;
                float angle = Vector2.Angle(playerDirection, Vector2.up);
                if (angle <= GameManager.Instance.angleCollisonEnterThreshHole)
                {


                    if(angle >= GameManager.Instance.besideThreshHole){
                        Debug.Log("riaf");
                        if(isRight)
                            CapybaraMain.Instance.animator.SetTrigger("fallRight");
                        else 
                            CapybaraMain.Instance.animator.SetTrigger("fallLeft");
                    }
                    CapybaraMain.Instance.animator.SetTrigger("touchGround");
                    animator.SetTrigger("hit");
                    CapybaraMain.Instance.StopMove();
                    Debug.Log("OK!");
                    this.StopMove();
                    if(gameObject != GameManager.Instance.oldCarpet)
                    {
                        if (angle <= GameManager.Instance.perfectJumpThreshHole)
                        {
                            ScoreController.Instance.AddScore(2);// fix
                            Debug.Log("Perfect!");
                        }
                        else
                        {
                            ScoreController.Instance.AddScore(1);
                        }
                    }    
                   
                    ScoreController.Instance.CheckGift(CapybaraMain.Instance.transform.localPosition + Vector3.up * InstantiateGameObject.Instance.carpetHeight * 1.5f);
                   
                    CapybaraMain.Instance.LandingSuccessful(this.gameObject);
                   


                }
                else
                {
                    if (GameManager.Instance.isShield)
                    {
                        ScoreController.Instance.shield.SetActive(false);
                        transform.DOKill();
                        MoveBack(0.5f);
                        Debug.Log("Lose");
                        CapybaraMain.Instance.LandingSuccessful(this.gameObject);
                        return;
                    }
                    else
                    {
                        GameManager.Instance.gameOver = true;
                        
                        this.StopMove();
                        int dimensity = (isRight) ? 1 : -1;
                        CapybaraMain.Instance.Die(dimensity);
                        //GameManager.Instance.GameOver();
                        //CapybaraMain.Instance.StopMove();
                        transform.GetChild(0).DOKill();
                    }
                }
            }

        }

        public void MoveToCenter(Vector3 startPos, Vector3 targetPos, float step)
        {
            startPosition = startPos;
            isMoving = true;

            moveTime = Random.Range(GameManager.Instance.startTime * step, GameManager.Instance.endTime * step);
            this.transform.localPosition = startPos;
            transform.DOLocalMove(targetPos, moveTime)
            .SetEase(this.easeType)
             .OnComplete(() =>
             {
                 isMoving = false;
                // animator.SetTrigger("hitTargetPoint");
                 StopMove();
             });

        }
        public void StopMove()
        {
            transform.DOKill();
            StickMoveBack();
            isMoving = false;

        }

        private void StickMoveBack()
        {
           
            Transform stick = transform.GetChild(0);
            Vector3 oldPos = new Vector3(stick.localPosition.x + 5f, stick.localPosition.y, stick.localPosition.z);
            transform.GetChild(0).DOLocalMove(oldPos, this.moveTime * 0.35f)
            .SetEase(this.easeType);
        }

        private void MoveBack(float time)
        {
            
            transform.DOLocalMove(startPosition, time)
           .SetEase(this.easeType)
            .OnComplete(() =>
            {
               
                isMoving = false;
               // StopMove();

            });

        }



    }


}
