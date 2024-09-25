
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
        public Vector2 bottomArea,topArea;             
        public float speed = 1.0f; 
        public float stopTime = 0;
        
        private Vector2 targetPosition; 
        private float moveTime; 
        private SpriteRenderer spriteRenderer;
        private bool isPause = false, isGetPos = false;
        void Start()
        {           
            targetPosition = GetRandomPosition();
            spriteRenderer = whole.GetComponent<SpriteRenderer>();
            GameManager.Instance.pause += FishPause;
            GameManager.Instance.unPause += FishUnPause;
        }

        void Update()
        {
            if (GameManager.Instance.gameState == GameState.Fishing || GameManager.Instance.gameState == GameState.Start)
            {
                if (!isCatch && !isPause)
                {                  
                    transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                    if ((Vector2)transform.position == targetPosition && !isGetPos)
                    {
                        isGetPos = true;
                        StartCoroutine(StartGetRandomPosition());
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
        }
        private void FishPause()
        {
            isPause = true;
        }
        private void FishUnPause()
        {
            isPause = false;
        }

        private Vector2 GetRandomPosition()
        {
            float randomX = Random.Range(bottomArea.x, topArea.x);
            float randomY = Random.Range(bottomArea.y, topArea.y);
            return new Vector2(randomX, randomY);
        }
        private IEnumerator StartGetRandomPosition()
        {           
            yield return new WaitForSeconds(stopTime);
            targetPosition =  GetRandomPosition();
            isGetPos = false;
        }
        private void Slice(Vector3 direction, Vector3 position, float force)
        {                       
            fishCol.enabled = false;
            whole.SetActive(false);

            sliced.SetActive(true);
                    
            Rigidbody2D[] slices = sliced.GetComponentsInChildren<Rigidbody2D>();
            
            foreach (Rigidbody2D slice in slices)
            {            
                slice.velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
                slice.AddForceAtPosition(direction * force, position, ForceMode2D.Impulse);              
            }
            UISlash.Instance.UpdateScore(point);
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
                    Vector2 collisionDirection = (transform.position - collision.transform.position).normalized;
                    collisionDirection.y = 2.5f;
                    rb.AddForce(collisionDirection * 2, ForceMode2D.Impulse);
                }
            }
        }
        private void OnDestroy()
        {
            GameManager.Instance.pause -= FishPause;
            GameManager.Instance.unPause -= FishUnPause;
        }
    }
}
