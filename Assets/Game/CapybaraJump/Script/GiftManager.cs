using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CapybaraMain;
namespace CapybaraJump
{
    public class GiftManager : MonoBehaviour
    {
        [SerializeField] private CoinFx coinFx;
        [SerializeField] private List<GameObject> listItem = new List<GameObject>();
        [SerializeField] Text  heartCount;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == GameManager.Instance.CapybaraMain_LayerIndex)
            {
                if (type == null) return;

                if (type == 1)
                {
                    coinFx.PlayFx(() =>
                    {
                        Manager.Instance.SetHeart(Manager.Instance.GetHeart() + 1);
                        heartCount.text = Manager.Instance.GetHeart().ToString();
                    }, 0, Camera.main.WorldToScreenPoint(CapybaraMain.Instance.transform.position), 1);
                }
                this.gameObject.SetActive(false);
            }
        }

    }

}

