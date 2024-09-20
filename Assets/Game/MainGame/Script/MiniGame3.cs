using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraMain
{
    public class MiniGame3 : BaseID
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        
        public void UserItemInMiniGame()
        {
            Manager.Instance.UpdateDataInFile(this);
        }
    }
}