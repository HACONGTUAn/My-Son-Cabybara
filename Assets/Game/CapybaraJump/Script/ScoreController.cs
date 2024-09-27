using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CapybaraJump
{
    public class ScoreController : MonoBehaviour
    {
        // Start is called before the first frame update
        public static ScoreController Instance { get; private set; }
        public int score = 0;
        public Text text;
        public GiftManager gift;
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
            text.text = score + " M";

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddScore(int scoreToAdd)
        {
            score += scoreToAdd;
            if (score < 0) score = 0;
            text.text = score + " M";

        }
        public void ResetScore()
        {
            score = 0;
            text.text = score + " M";
            
        }


        public void CheckGift(Vector3 position)
        {
            if (score%20 == 0)
            {
                gift.transform.localPosition = position;
                gift.type = 1;
                gift.gameObject.SetActive(true);

            }
           
        }
    }

}

