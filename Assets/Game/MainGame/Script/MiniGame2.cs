using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraMain
{
    public class MiniGame2 : BaseID
    {
        //public TMP_Text textButton1;
        //public TMP_Text soluongBtn1;

        //public TMP_Text textButton2;
        //public TMP_Text soluongBtn2;
        
        private void Awake()
        {
            items = Manager.Instance.ReadDataInFile(this);

            
            //textButton1.text = items[0].name;
            //soluongBtn1.text = items[0].quantity.ToString();

            //textButton2.text = items[1].name;
            //soluongBtn2.text = items[1].quantity.ToString();

        }

       public void UserItemInMiniGame()
        {
            Manager.Instance.UpdateDataInFile(this);
        }
        
        public void ClickBtn()
        {
            //items[0].quantity --;
            //soluongBtn1.text = items[0].quantity.ToString();
            //UserItemInMiniGame();
            //Debug.Log("clickBtn");
        }

    }

}