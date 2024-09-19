using System;
using UnityEngine;
using DG.Tweening;
namespace Merge
{
    enum AnimShowPopUp
    {
        None,
        MoveMent,
        ScalePunch
    }
    public abstract class PopupUI : MonoBehaviour
    {
        protected UIManager uiManager;
        protected Action onClose;
        public static event Action<PopupUI> OnDestroyPopup;
        public static event Action<PopupUI> OnHide;
        public static event Action<PopupUI> OnShow;
        public bool isCache = false;
        public bool isShowing { get; protected set; }
        public virtual void Initialize(UIManager manager)
        {
            this.uiManager = manager;
            gameObject.SetActive(false);
            isShowing = false;
        }
        public virtual void Show(Action onClose)
        {
            this.onClose = onClose;
            isShowing = true;
        
            gameObject.SetActive(true);

            OnShow?.Invoke(this);
        }
        public virtual void Hide()
        {
            AudioManager.Instance.PlayOneShot("ClickSound", 1f);
            if(Time.timeScale == 0) { Time.timeScale = 1; }
            isShowing = false;
            gameObject.SetActive(false);
            onClose?.Invoke();
            onClose = null;
            OnHide?.Invoke(this);
            if (!isCache)
            {
                OnDestroyPopup?.Invoke(this);
                OnPopupDestroyed();
            }
        }
        protected virtual void OnPopupDestroyed()
        {
            
        }
    }
}