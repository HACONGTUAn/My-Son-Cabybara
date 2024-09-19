    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Merge
{
    public class UIBoosterConfirmPanel : MonoBehaviour
    {
        [SerializeField] Text decsText;
        [SerializeField] Button yesButton;
        [SerializeField] Button noButton;
        private event Action<bool> callBack;
        public void Active(string content, Action<bool> callBack)
        {
            gameObject.SetActive(true);
            decsText.text = content;
            this.callBack = callBack;
            yesButton.onClick.AddListener(Confirm);
            noButton.onClick.AddListener(Deny);
        }

        private void Deny()
        {
            callBack?.Invoke(false);
            gameObject.SetActive(false);
        }

        private void Confirm()
        {
            callBack?.Invoke(true);
            gameObject.SetActive(false);
        }
    }

}