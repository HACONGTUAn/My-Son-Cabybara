using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraMain
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]private HomeUI homeUI;
        [SerializeField]private ShopUI shopUI;
        [SerializeField]private StotyUI stotyUI;

        private BaseUI currentUI = null;
        // Start is called before the first frame update
        void Start()
        {
            ShowHomeUI();
        }

       
        public void ShowHomeUI()
        {
            ShowPanel(homeUI);
        }
        public void ShowShopUI()
        {
            ShowPanel(shopUI);
        }
        public void ShowStoryUI()
        {
            ShowPanel(stotyUI);
        }
        private void ShowPanel(BaseUI currentPanel)
        {
            if(currentUI == null)
            {
                currentUI = currentPanel;
                currentUI.gameObject.SetActive(true);
                return;
            }
            else
            {
                currentUI.gameObject.SetActive(false);
            }
            currentUI = currentPanel;
            currentUI.gameObject.SetActive(true);
        }
    }
}

