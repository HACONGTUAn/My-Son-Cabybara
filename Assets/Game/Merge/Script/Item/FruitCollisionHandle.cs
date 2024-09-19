using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Merge
{
    public class FruitCollisionHandle : MonoBehaviour
    {
        [SerializeField] public Fruit owner { get; private set; }
        public void Initialize(Fruit fruit)
        {
            owner = fruit;
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            owner.OnCollision(other);
        }
    }
}
