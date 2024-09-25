using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CapybaraJump
{
    public class BoosterBtn : MonoBehaviour
    {
        
        public BoosterType boosterType;
        public GameObject count;
        public GameObject buy;
        public GameObject startBtn;
        private Button button;
        public BuyBooster buyBooster;
        private int amount;
        private void Start() 
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(UseBooster);
            Refresh();
        }
        public void resetStart()
        {
            gameObject.SetActive(amount > 0);
        }
        public void Refresh()
        {
            if ((int)boosterType == 0)
            {
                amount = GameManager.Instance.minigame.items[0].quantity;
            }
            if ((int)boosterType == 1)
            {

                amount = GameManager.Instance.minigame.items[1].quantity;
            }
            count.SetActive(amount > 0);
            count.GetComponentInChildren<Text>().text = amount.ToString();
            buy.SetActive(amount <= 0);
            gameObject.SetActive(true);
        }
        private void UseBooster()
        {
            if (amount > 0 && GameManager.Instance.IsStart && !GameManager.Instance.gameOver)
            {
                if((int)boosterType == 0 && !GameManager.Instance.isBoost)
                {
                    GameManager.Instance.minigame.items[0].quantity -= 1;
                    GameManager.Instance.Booster();
                }
                if((int)boosterType == 1 && !GameManager.Instance.isShield) {

                    GameManager.Instance.minigame.items[1].quantity -= 1;
                    GameManager.Instance.Shield();
                }
                Refresh();
                gameObject.SetActive(amount > 0);

            }
            else if(!GameManager.Instance.IsStart)
            {
                startBtn.SetActive(false);
                buyBooster.gameObject.SetActive(true);
                buyBooster.boosterType = boosterType;
                buyBooster.UpdateCount();
                buyBooster.AnimatedUI();
            }
        }
    }
}
