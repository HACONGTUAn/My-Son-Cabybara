using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Fishing
{
    public class UIStart : MonoBehaviour, IBaseUI
    {
        public void Initialize()
        {

            gameObject.SetActive(true);

        }
        public void Clear()
        {

            gameObject.SetActive(false);
        }
    }
}
