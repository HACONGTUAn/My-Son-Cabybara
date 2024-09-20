using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Merge
{
    public class CameraManager : Singleton<CameraManager>
    {
        [HideInInspector] public Camera mainCamera;
        public Camera uiCamera;
        private void Start() 
        {
            mainCamera = Camera.main;
        }
        public void SetView(Bounds bounds)
        {
            float sizeX = bounds.size.x;
            float orthographicSize = sizeX * Screen.height / Screen.width * 0.5f;
            mainCamera.orthographicSize = orthographicSize;
        }
    }
}