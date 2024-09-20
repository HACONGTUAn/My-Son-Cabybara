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
        }
        private void FireFish()
        {
            StartCoroutine(StartFireFish());
        }
        private IEnumerator StartFireFish()
        {
            if (GameManager.Instance.gameState == GameState.SlashFish)
            {
                
                for (int i = 0; i < transform.childCount; i++)
                {
                   
                    yield return new WaitForSeconds(Random.Range(1f, 1.5f));
                    GameObject fish = transform.GetChild(i).gameObject;     
                    Rigidbody2D rb = fish.GetComponent<Rigidbody2D>();
                    if(rb == null) rb = fish.AddComponent<Rigidbody2D>();
                    rb.drag = 1;
                    rb.gravityScale = 0.8f;              
                    fish.transform.GetChild(0).gameObject.SetActive(true);
                    //Logic can bang ban ca
                    fish.transform.localPosition = new Vector3(Random.Range(-7, 0.8f), 0, 0);
                    fish.transform.localRotation = Quaternion.identity;
                    rb.AddForce(new Vector2(Random.Range(-1,1), Random.Range(17f, 22f)),ForceMode2D.Impulse);
                    rb.AddTorque(2);
                }
                ClearBucket();
            }
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
        }
    }
}
