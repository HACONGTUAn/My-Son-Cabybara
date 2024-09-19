using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraJump
{
    public class GiftManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> listItem = new List<GameObject>();
        public static GiftManager Instance;
        public int type;
        // Start is called before the first frame update
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

       /* private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == GameManager.Instance.CapybaraMain_LayerIndex)
            {
                if (type == null) return;

                if (type == 1)
                {
                    //GameManager.Instance.isBoost = true;
                }

                if (type == 2)
                {
                   // GameManager.Instance.isShield = true;
                   // ScoreController.Instance.shield.SetActive(true);
                }
                this.gameObject.SetActive(false);
            }
        }*/

    }

}

