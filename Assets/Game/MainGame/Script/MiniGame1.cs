using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CapybaraMain { 
public class MiniGame1 : BaseID
{
       
        
        private void Start()
        {
            items = Manager.Instance.ReadDataInFile(this);
        }

       public void UserItemInMiniGame()
        {
            Manager.Instance.UpdateDataInFile(this);
        }
        
        public void ClickBtn()
        {
          
        }
        public void AddItem(int type, int value)
        {
            Merge.GameRes.AddRes(new Merge.DataTypeResource(Merge.RES_type.BOOSTER, type), value, "");
        }

}

}