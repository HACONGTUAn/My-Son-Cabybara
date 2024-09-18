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
        public float moveTime;
        [SerializeField] private Ease easeType;
        private Vector3 targetPos;

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
            this.targetPos += Vector3.up * InstantiateGameObject.Instance.carpetHeight*step;
            transform.DOMove(this.targetPos, time)
                .SetEase(this.easeType);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}
