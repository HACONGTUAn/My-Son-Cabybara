using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Merge
{
    public class CameraManager : Singleton<CameraManager>
    {
        public Camera mainCamera;
        public Camera uiCamera;
        public void SetView(Bounds bounds)
        {
            float sizeX = bounds.size.x;
            float orthographicSize = sizeX * Screen.height / Screen.width * 0.5f;
            mainCamera.orthographicSize = orthographicSize;
        }
    }
}