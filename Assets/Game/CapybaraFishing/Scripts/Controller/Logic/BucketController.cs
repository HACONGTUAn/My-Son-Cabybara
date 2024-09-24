using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class BucketController : MonoBehaviour
    {

        void Start()
        {
            GameManager.Instance.slashEvent += FireFish;
            GameManager.Instance.fishingEvent += ClearBucket;
        }
        private void FireFish()
        {
            StartCoroutine(StartFireFish());
        }
        private IEnumerator StartFireFish()
        {
            if (GameManager.Instance.gameState == GameState.SlashFish)
            {
                
                foreach(Transform fishTransform in transform)
                {
                   
                    yield return new WaitForSeconds(Random.Range(1f, 1.5f));
                    GameObject fish = fishTransform.gameObject;     
                    Rigidbody2D rb = fish.GetComponent<Rigidbody2D>();
                    if(rb == null) rb = fish.AddComponent<Rigidbody2D>();
                    rb.drag = 1;
                    rb.gravityScale = 0.8f;              
                    fishTransform.GetChild(0).gameObject.SetActive(true);
                    //Logic can bang ban ca
                    fishTransform.localPosition = new Vector3(Random.Range(-7, 0.8f), 0, 0);
                    fishTransform.localRotation = Quaternion.identity;
                    rb.AddForce(new Vector2(Random.Range(-1,1), Random.Range(15f, 20f)),ForceMode2D.Impulse);
                    rb.AddTorque(Random.Range(0,2f));
                }
                StartCoroutine(ChangeEndState());
            }
        }
        private IEnumerator ChangeEndState()
        {
            yield return new WaitForSeconds(3); 
            UISlash.Instance.EndGame();
        }
        private void ClearBucket()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        private void OnDestroy()
        {
            GameManager.Instance.slashEvent -= FireFish;
            GameManager.Instance.fishingEvent -= ClearBucket;
        }
    }
}
