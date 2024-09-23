using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CapybaraJump
{
    public class ScoreController : MonoBehaviour
    {
        // Start is called before the first frame update
        public static ScoreController Instance { get; private set; }
        public int score = 0;
  
        [SerializeField] private GiftManager gift;
        public GameObject shield;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        void Start()
        {
          

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddScore(int scoreToAdd)
        {
            score += scoreToAdd;
            if (score < 0) score = 0;
          

        }


        /*public void CheckGift(Vector3 position)
        {
            if (score == 3)
            {
                gift.transform.position = position;
                gift.type = 2;
                gift.gameObject.SetActive(true);

            }
            if (score == 5)
            {
                gift.transform.position = position;
                gift.type = 1;
                gift.gameObject.SetActive(true);
            }
        }*/
    }

}

