using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CapybaraJump
{
    public class ScoreController : MonoBehaviour
    {
        // Start is called before the first frame update
        public static ScoreController Instance { get; private set;}
        public int score = 0;
        public TextMeshProUGUI text;
        [SerializeField] private GameObject gift;

        void Awake(){
            if(Instance == null){
                Instance = this;
            }
        }
        void Start()
        {
            text.text = score + " M";
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void AddScore(int scoreToAdd){
            score += scoreToAdd;
            if( score < 0 ) score = 0;
            text.text = score + " M";
            
        }


        public void CheckGift(Vector3 position){
            if (score == 5){
                gift.transform.position = position;
                gift.SetActive(true);
                
            }
        }
    }

}

