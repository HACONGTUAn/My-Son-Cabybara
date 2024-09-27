using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CapybaraJump
{

    public class HeartAdsFill : MonoBehaviour
    {
       public Image fillImage; // Gán hình ảnh bạn muốn fill (phải có component Image)

       [SerializeField] private GameObject gameOverPanel;
       [SerializeField] private GameObject revivePanel;
        
        private void Start()
        {
            // Bắt đầu quá trình fill từ 0 đến 1 trong 5 giây
            
        }


        public void StartLoading(){
            
            StartCoroutine(FillImageOverTime(5f));
        }
        IEnumerator FillImageOverTime(float duration)
        {
            float elapsed = 0f;
            fillImage.fillAmount = 1f; // Bắt đầu từ 0

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                fillImage.fillAmount = Mathf.Clamp01(1f - (elapsed / duration)); // Tính toán lượng fill
               
                yield return null; // Chờ đến khung hình tiếp theo
            }
            
            gameOverPanel.SetActive(true);
            revivePanel.SetActive(false);
            fillImage.fillAmount = 1f; // Đảm bảo giá trị cuối cùng là 1
        }
    }


}
