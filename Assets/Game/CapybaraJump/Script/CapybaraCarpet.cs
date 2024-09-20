using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CapybaraJump
{

    public class CapybaraCarpet : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private Ease easeType;
        [SerializeField] private float moveTime;
        public bool isMoving = false;
        private Vector3 startPosition;


        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == GameManager.Instance.CapybaraShield_LayerIndex)
            {
                MoveBack(0.5f);
                GameManager.Instance.isShield = false;
                ScoreController.Instance.shield.SetActive(false);
                return;
            }
            if (collision.gameObject.layer == GameManager.Instance.CapybaraMain_LayerIndex)
            {
                Vector2 playerDirection = collision.transform.position - transform.GetChild(1).transform.position;
                float angle = Vector2.Angle(playerDirection, Vector2.up);
                if (angle <= GameManager.Instance.angleCollisonEnterThreshHole)
                {
                    this.StopMove();
                    if (angle <= GameManager.Instance.perfectJumpThreshHole)
                    {
                        ScoreController.Instance.AddScore(1);// fix
                        Debug.Log("Perfect!");
                    }
                    else
                    {
                        ScoreController.Instance.AddScore(1);
                    }
                  //  ScoreController.Instance.CheckGift(CapybaraMain.Instance.transform.position + Vector3.up * InstantiateGameObject.Instance.carpetHeight * 1.5f);
                    CapybaraMain.Instance.LandingSuccessful();

                }
               else
                {
                    Debug.Log("Lose");
                    GameManager.Instance.GameOver();
                    this.StopMove();

                }
            }

        }

        public void MoveToCenter(Vector3 startPos, Vector3 targetPos, float step)
        {
            startPosition = startPos;
            isMoving = true;

            moveTime = Random.Range(GameManager.Instance.startTime * step, GameManager.Instance.endTime * step);
            this.transform.position = startPos;
            transform.DOLocalMove(targetPos, moveTime)
            .SetEase(this.easeType)
             .OnComplete(() =>
             {
                 isMoving = false;
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
                StopMove();

            });

        }



    }


}
