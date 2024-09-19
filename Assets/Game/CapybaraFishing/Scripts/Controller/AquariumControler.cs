using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class AquariumControler : MonoBehaviour
    {
        [SerializeField] private GameObject fish;
        void Start()
        {

        }

        private void SpawmFish()
        {
            GameObject newFish = Instantiate(fish,transform);

        }
    }
}
