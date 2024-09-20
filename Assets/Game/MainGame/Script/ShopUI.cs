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
        // Start is called before the first frame update
   void OnEnable()
        {
            _heartUi.text = Manager.Instance.GetHeart().ToString();
            _ticketUi.text = Manager.Instance.GetTicket().ToString();
        }

    // Update is called once per frame
    void Update()
    {
        
    }
}

}