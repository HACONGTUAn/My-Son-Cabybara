﻿using System.Collections;
using UnityEngine;

namespace Fishing
{
    public class HookController : MonoBehaviour
    {
        public float rotateSpeed = 50f;
        public float minRotateAngle = -70f;
        public float maxRotateAngle = 70f;
        public float hookForce = 600f;
        public float returnForce = 600f;
        public float returnTime = 1;
        [SerializeField]
        private GameObject itemHolder;
        [SerializeField]
        private GameObject bucket;


        private bool isShooting = false;
        private bool isReturning = false;   
        private bool isCatched = false;
        private LineRenderer lineRenderer;
        private Vector3 originalPosition;      
        private Coroutine autoReturn;
        private Rigidbody2D rb;
        private float totalMass = 0.2f;


        void Start()
        {           
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            GameManager.Instance.fishingEvent += OnFishingHandle;
        }
        private void OnFishingHandle()
        {
            Debug.Log("Oke");
            StartCoroutine(OnFishing());
        }
        private IEnumerator OnFishing()
        {
            while (GameManager.Instance.gameState == GameState.Fishing)
            {
                if (!isShooting && !isReturning)
                {
                    float angle = Mathf.PingPong(Time.time * rotateSpeed, maxRotateAngle - minRotateAngle) + minRotateAngle;
                    transform.localRotation = Quaternion.Euler(0, 0, angle);
                }

                if (Input.GetKeyDown(KeyCode.Space) && !isShooting && !isReturning)
                {
                    isShooting = true;
                    originalPosition = transform.localPosition;
                    rb = gameObject.AddComponent<Rigidbody2D>();
                    rb.gravityScale = 0;
                    rb.drag = 0.5f;
                    rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                    rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
                    rb.AddForce(transform.up * -hookForce);
                    autoReturn = StartCoroutine(AutoReturnAfterDelay());

                } // sua lai dieu kien ban cau

                if (isReturning)
                {
                    StopCoroutine(autoReturn);
                    Vector2 directionToReturn = (transform.parent.position - transform.position).normalized;
                    rb.mass = totalMass;
                    rb.AddForce(directionToReturn * returnForce * Time.deltaTime);
                    if (Vector2.Distance(transform.position, transform.parent.position) < 0.5f)
                    {
                        transform.localPosition = originalPosition;
                        rb.velocity = Vector2.zero;
                        Destroy(rb);
                        totalMass = 0.2f;
                        if (itemHolder.transform.childCount > 0)
                            StartCoroutine(CatchFishAnim());
                        else isShooting = false;
                        isReturning = false;
                    }

                }

                lineRenderer.SetPosition(0, transform.parent.position);
                lineRenderer.SetPosition(1, transform.position);
                yield return null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isCatched && isShooting && other.CompareTag("Item"))
            {
                isCatched = true;
                Transform fishTrans = other.transform.parent;
                fishTrans.parent = itemHolder.transform;
                fishTrans.localPosition = Vector3.zero;
                totalMass += other.transform.parent.GetComponent<FishController>().massFish;
                other.enabled = false;
                isReturning = true;
                rb.velocity = Vector2.zero;
            }
        }

        private IEnumerator AutoReturnAfterDelay()
        {
            yield return new WaitForSeconds(returnTime);          
            isReturning = true;
        }

        private IEnumerator CatchFishAnim()
        {
            Animator animator = itemHolder.GetComponent<Animator>();
            Transform originParent = itemHolder.transform.parent;
            Vector3 originPos = originParent.position;

            itemHolder.transform.SetParent(originParent.parent); // itemHolder chua ca dat ra ngoai de chay anim
            animator.SetTrigger("CatchFish");
            yield return new WaitForSeconds(1f);
            GameObject fish = itemHolder.transform.GetChild(0).gameObject;
            fish.transform.SetParent(bucket.transform);
            fish.transform.rotation = Quaternion.identity; // Chuyen ca qua parent khac

            itemHolder.transform.SetParent(originParent); // reset lai vi tri item holder
            itemHolder.transform.position = originPos;
            isReturning = false;
            isShooting = false;
            isCatched = false;
        }
        private void OnDestroy()
        {
            GameManager.Instance.fishingEvent -= OnFishingHandle;
        }
    }
}
