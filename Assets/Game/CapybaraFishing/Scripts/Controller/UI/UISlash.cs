using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class UISlash : MonoBehaviour, IBaseUI
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
