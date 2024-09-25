using Fishing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class BladeController : MonoBehaviour
    {
        public float sliceForce = 30f;
        public float minSliceVelocity = 0.01f;

        private Camera mainCamera;
        private Collider2D sliceCollider;
        private TrailRenderer sliceTrail;

        public Vector3 direction { get; private set; }
        public bool slicing { get; private set; }

        private void Awake()
        {
            mainCamera = Camera.main;
            sliceCollider = GetComponent<Collider2D>();
            sliceTrail = GetComponentInChildren<TrailRenderer>();
            
        }
        private void Start()
        {
            GameManager.Instance.slashEvent += BladeActive;
        }
        private void OnEnable()
        {
            StopSlice();
        }

        private void OnDisable()
        {
            StopSlice();
        }

        private void BladeActive() 
        {
            StartCoroutine(StartBlade());
        }

       
        private IEnumerator StartBlade()
        {
            while (GameManager.Instance.gameState == GameState.SlashFish)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartSlice();
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    StopSlice();
                }
                else if (slicing)
                {
                    ContinueSlice();
                }
                yield return null;
            }
            StopSlice();
        }
        private void StartSlice()
        {
            Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0f;
            transform.position = position;

            slicing = true;
            sliceCollider.enabled = true;
            sliceTrail.enabled = true;
            sliceTrail.Clear();
        }

        private void StopSlice()
        {
            slicing = false;
            sliceCollider.enabled = false;
            sliceTrail.enabled = false;
        }

        private void ContinueSlice()
        {
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0f;            
            direction = newPosition - transform.position;

            transform.position = newPosition;
        }

        private void OnDestroy()
        {
            GameManager.Instance.slashEvent -= BladeActive;
        }
    }
}
