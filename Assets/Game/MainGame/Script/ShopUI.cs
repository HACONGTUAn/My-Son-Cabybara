using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CapybaraMain
{


public class ShopUI : BaseUI
{
        [SerializeField] private Text _heartUi;
        [SerializeField] private Text _ticketUi;
        [Header("Add List Ticket in Shop")]
        [SerializeField] private List<TicketInShop> _listTicket;
        [Header("Add List Pack Combo in Shop")]
        [SerializeField] private List<ComboItemInMiniGamePack> _packCombo;

        // Start is called before the first frame update
        private void OnEnable()
        {
            _heartUi.text = Manager.Instance.GetHeart().ToString();
            _ticketUi.text = Manager.Instance.GetTicket().ToString();
        }
        void Start()
        {
            BuyOnTheTicketButton();
            BuyOnThePackButton();
        }

        private void BuyOnThePackButton()
        {
            foreach (var _even in _packCombo)
            {
                _even.button.onClick.AddListener(() => BuyPack(_even._packItemMiniGame));
            }
        }
        // so luong phai la 3 minigame
        private void BuyPack(List<MiniGame> packItemMiniGame)
        {
            int sumPrice = 0;
            for (int i = 0; i < packItemMiniGame.Count; i++)
            {
                Debug.Log("id : " + packItemMiniGame[i].id
                          + " name : " + packItemMiniGame[i].nameMinigame +
                          " gia " + packItemMiniGame[i].price
                          );
                if (i < Manager.Instance._data.Count)
                {
                    if (packItemMiniGame[i].id == Manager.Instance._data[i].id)
                    {
                        Manager.Instance._data[i].items[0].quantity += packItemMiniGame[i].items[0].quantity;
                        Manager.Instance._data[i].items[1].quantity += packItemMiniGame[i].items[1].quantity;
                        Manager.Instance.WriteDataInFile();
                    }
                }

                sumPrice += packItemMiniGame[i].price;
            }
            Debug.Log("gia tien " + sumPrice);
        }

        private void BuyOnTheTicketButton()
        {
            foreach (var _even in _listTicket)
            {
                _even._button.onClick.AddListener(() => BuyTicket(_even._quantity, _even._price, _even._noAdv));
            }
        }

        private void BuyTicket(int quantity, float price, bool noAdv)
        {
            Debug.Log("So luong : " + quantity + " gia : " + price);

            _ticketUi.text = (Manager.Instance.GetTicket() + quantity).ToString();
            Manager.Instance.SetTicket(Manager.Instance.GetTicket() + quantity);
            if (noAdv)
            {
                Debug.Log("khong quang cao ");
            }
        }
    }

}