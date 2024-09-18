using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraMain
{ 
    public class HomeUI : BaseUI
    {
      public int index = 0;
    
      public void OnClickPlayGame()
        {
            if (! LoadingResources.Instance.keyValuePairs.ContainsKey(index)) {
                Debug.Log("No key");
                return;
            }
            Instantiate(LoadingResources.Instance.keyValuePairs[index]);
            this.gameObject.SetActive(false);
        }
    }
}
