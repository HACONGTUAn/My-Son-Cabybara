using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraMain
{
    public class MiniGame3 : BaseID
    {

        private void Start()
        {
            items = Manager.Instance.ReadDataInFile(this);
        }
        internal void UserItemInMiniGame()
        {
            Manager.Instance.UpdateDataInFile(this);
        }
    }
}