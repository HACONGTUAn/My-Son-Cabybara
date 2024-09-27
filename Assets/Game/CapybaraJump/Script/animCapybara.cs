using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraJump
{
    
    public class animCapybara : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        public void Ground(){
            GetComponent<Animator>().SetTrigger("touchGround");
        }
    }

}