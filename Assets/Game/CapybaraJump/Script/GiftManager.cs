using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraJump
{
    public class GiftManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D other){
            if (other.gameObject.layer == GameManager.Instance.CapybaraMain_LayerIndex)
            {
                this.gameObject.SetActive(false);
                GameManager.Instance.isBoost = true;
            }
        }
        
    }
    
}

