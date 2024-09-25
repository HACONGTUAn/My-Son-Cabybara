using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraMain
{
    public class MiniGame3 : BaseID
    {
     
        internal void UserItemInMiniGame()
        {
            Manager.Instance.UpdateDataInFile(this);
        }
    }
}