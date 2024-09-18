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
       
        
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == GameManager.Instance.CapybaraMain_LayerIndex)
            {
                Vector2 playerDirection = collision.transform.position - transform.GetChild(1).transform.position;
                float angle = Vector2.Angle(playerDirection, Vector2.up);
                if(angle <= GameManager.Instance.angleCollisonEnterThreshHole)
                {
                    this.StopMove();
                   
                }
                else
                {
                    this.StopMove();
                    
                }
            }
        }

        public void MoveToCenter(Vector3 startPos, Vector3 targetPos){
            moveTime = Random.Range(3f, 6f);
            this.transform.position = startPos;
            transform.DOLocalMove(targetPos, moveTime)
            .SetEase(this.easeType)
            /* .OnComplete(()=> {
                SpawnCarpet.Instance.UpdatePos();
            }) */;
           
        }
        public void StopMove(){
            transform.DOKill();
            StickMoveBack();
        }

        private void StickMoveBack()
        {
            Transform stick = transform.GetChild(0);
            Vector3 oldPos = new Vector3(stick.localPosition.x + 5f, stick.localPosition.y, stick.localPosition.z);
            transform.GetChild(0).DOLocalMove(oldPos, this.moveTime)
            .SetEase(this.easeType);
        }

        
    }


}
