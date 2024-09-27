using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace CapybaraJump
{
    public class CameraFollowController : MonoBehaviour
    {
        // Start is called before the first frame update
        public static CameraFollowController Instance { get; private set;}
        [SerializeField] private RectTransform background;
        public float moveTime;
        [SerializeField] private Ease easeType;
        public Vector3 targetPos;

         [SerializeField] private Vector3 camearaInitPosition;



        void Awake(){
            if(Instance == null){
                Instance = this;
            }
        }
        void Start()
        {
            targetPos = new(0f, 0f, -10f);
            
        }

        public void MoveUpperOneTime(int step, float time){
            float targetPosY = background.anchoredPosition.y - 10f;
            this.targetPos += Vector3.up * InstantiateGameObject.Instance.carpetHeight*step;
            transform.DOMove(this.targetPos, time)
                .SetEase(this.easeType);


            background.DOAnchorPosY(targetPosY, time).SetEase(this.easeType);
          
        }

        public void ResetCamera(){
            background.anchoredPosition = new Vector2(0 , 2544f);
            transform.localPosition = camearaInitPosition;
            targetPos = camearaInitPosition;

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
