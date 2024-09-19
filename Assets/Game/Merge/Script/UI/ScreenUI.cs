using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Merge
{
    public abstract class ScreenUI : MonoBehaviour
    {
        public static event Action<ScreenUI> OnDestroyScreen;
        public bool isCache;
        protected UIManager uiManager;
        public virtual void Initialize(UIManager uiManager)
        {
            this.uiManager = uiManager;
        }
        public virtual void Active()
        {
            gameObject.SetActive(true);
        }
        public virtual void Deactive()
        {
            gameObject.SetActive(false);
            if (!isCache)
            {
                OnDestroyScreen?.Invoke(this);
                OnScreenDestroyed();
            }
        }
        protected virtual void OnScreenDestroyed() { }
    }
}