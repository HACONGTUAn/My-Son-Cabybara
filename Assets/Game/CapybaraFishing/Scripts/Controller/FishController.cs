using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Fishing
{
    public class FishController : MonoBehaviour
    {
        
        [SerializeField] private GameObject whole;
        [SerializeField] private GameObject sliced;
        [SerializeField] private Collider2D fishCol;
        public float massFish;
        public int point = 100;
        public int slashCount = 1;
        public bool isCatch = false;

        public float speed = 1.0f;        
        private Vector2 targetPosition; 
        private float moveTime; 
        private SpriteRenderer spriteRenderer;
        void Start()
        {           
            targetPosition = GetRandomPosition();
            spriteRenderer = whole.GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if (GameManager.Instance.gameState == GameState.Fishing && !isCatch)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                if ((Vector2)transform.position == targetPosition)
                {
                    targetPosition = GetRandomPosition();
                }
                
                if (targetPosition.x > transform.position.x)
                {
                    spriteRenderer.flipX = false; 
                }
                else if (targetPosition.x < transform.position.x)
                {
                    spriteRenderer.flipX = true; 
                }
            }
        }

        private Vector2 GetRandomPosition()
        {
          
            float randomX = Random.Range(-3.0f, 3.0f); 
            float randomY = Random.Range(-4f, 0f); 
            return new Vector2(randomX, randomY);
        }
        private void Slice(Vector3 direction, Vector3 position, float force)
        {
            int d = -1;
            fishCol.enabled = false;
            whole.SetActive(false);

            sliced.SetActive(true);
                    
            Rigidbody2D[] slices = sliced.GetComponentsInChildren<Rigidbody2D>();

            foreach (Rigidbody2D slice in slices)
            {            
                slice.velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
                slice.AddForceAtPosition(d * direction * force, position, ForceMode2D.Impulse);
                d += 2;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Blade" && slashCount > 0) 
            {
                slashCount--;
                if (slashCount == 0)
                {
                    BladeController blade = collision.GetComponent<BladeController>();
                    Slice(blade.direction, blade.transform.position, blade.sliceForce);
                }
                else
                {
                    Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
                    rb.AddForce(Vector2.up * 3, ForceMode2D.Impulse);
                }
            }
        }
    }
}
