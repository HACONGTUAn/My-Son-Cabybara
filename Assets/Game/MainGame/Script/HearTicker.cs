using UnityEngine;
using UnityEngine.UI;

namespace CapybaraMain
{
    public class HearTicker : MonoBehaviour
    {
        public static HearTicker Instance { get; private set; }
        public Text Heart;
        public Text Ticket;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            ChangeValue();
        }
        public void ChangeValue()
        {
            Heart.text = Manager.Instance.GetHeart().ToString();
            Ticket.text = Manager.Instance.GetTicket().ToString();
        }
    }
}