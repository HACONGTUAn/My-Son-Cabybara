using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Merge
{
    public class SplashLoadingCtr : MonoBehaviour
    {
        private const float perStep = 0.75f;
        public Image imgFill;
        public Image imgWatermelon;
        public Text txtFill;
        private string strFill;
        private int isWaitConfig = 0;
        private float toTimeWait = 2;
        private float minWait = 2;
        private int stateGetCf = 0;
        float timestep1 = 0;
        float timestep2 = 0;
        float timeRun = 0;
        float kTime = 1;
        float tcheckads = 0;
        private RectTransform imgWatermelonRectTransform;
        private Vector2 watermelonStartPos;
        private Vector2 watermelonEndPos;

        // Start is called before the first frame update
        void Start()
        {
            imgWatermelonRectTransform = imgWatermelon.GetComponent<RectTransform>();
            watermelonStartPos = imgWatermelonRectTransform.anchoredPosition;
            watermelonEndPos = new Vector2(watermelonStartPos.x + 428, watermelonStartPos.y);
        }

        public void showLoading(float time = 2, int isWaitCondition = 0, float minwait = 2)
        {
            RectTransform rc = GetComponent<RectTransform>();
            rc.anchorMin = new Vector2(0, 0);
            rc.anchorMax = new Vector2(1, 1);
            rc.sizeDelta = new Vector2(0, 0);
            rc.anchoredPosition = Vector2.zero;
            rc.anchoredPosition3D = Vector3.zero;
            // SDKManager.Instance.isAllowShowFirstOpen = false;

            toTimeWait = time;
            this.minWait = minwait;
            if (this.minWait < 2)
            {
                this.minWait = 2;
            }
            isWaitConfig = isWaitCondition;
            stateGetCf = 0;
            if (isWaitConfig == 1)
            {
                //if (GameHelper.checkLvXaDu())
                //{
                //    toTimeWait = this.minWait;
                //}
            }
            if (toTimeWait < 2)
            {
                toTimeWait = 2;
            }

            //FIRhelper.CBGetconfig += onGetConfig;
            timestep1 = perStep * toTimeWait;
            timestep2 = toTimeWait - timestep1;
            timeRun = 0;
            imgFill.fillAmount = 0;
            strFill = txtFill.text;
            txtFill.text = strFill + " 0%";
            kTime = 1;

            //StartCoroutine(sssttt());
        }

        IEnumerator sssttt()
        {
            yield return new WaitForSeconds(2.5f);
            int tmptoTimeWait = 20;
            Debug.Log("aaaaa 2=" + tmptoTimeWait);
            if (tmptoTimeWait < 2)
            {
                tmptoTimeWait = 2;
            }
            if (Mathf.Abs(tmptoTimeWait - toTimeWait) >= 0.5f)
            {
                timeRun = tmptoTimeWait * timeRun / toTimeWait;
                toTimeWait = tmptoTimeWait;
                timestep1 = perStep * toTimeWait;
                timestep2 = toTimeWait - timestep1;
            }
            yield return new WaitForSeconds(2.5f);
            stateGetCf = 2;
            kTime = 10;
        }

        public void onGetConfig()
        {
            if (isWaitConfig != 0)
            {
                stateGetCf = 1;
            }
            int tmptoTimeWait = PlayerPrefs.GetInt("time_splash_scr", 4);
            /*Debug.Log("aaaaa 2=" + tmptoTimeWait);
            if (tmptoTimeWait < 2)
            {
                tmptoTimeWait = 2;
            }
            if (Mathf.Abs(tmptoTimeWait - toTimeWait) >= 0.5f)
            {
                timeRun = tmptoTimeWait * timeRun / toTimeWait;
                toTimeWait = tmptoTimeWait;
                timestep1 = perStep * toTimeWait;
                timestep2 = toTimeWait - timestep1;
            }*/
        }

        private void Update()
        {
            // if (stateGetCf == 1 && timeRun >= minWait)
            // {
            //     stateGetCf = 2;
            //     //kTime = 3;
            // }
            /*if (stateGetCf == 0 && timeRun >= minWait && toTimeWait > 3)
            {
                tcheckads += Time.deltaTime;
                if (tcheckads >= 0.5f)
                {
                    tcheckads = 0;
                    if (AdsHelper.Instance.hasFullLoaded(AdsBase.PLFullSplash, false))
                    {
                        stateGetCf = 2;
                        kTime = 10;
                    }
                }
            }*/
            Time.timeScale = 1;
            timeRun += Time.deltaTime * kTime;
            if (timeRun <= timestep1)
            {
                imgFill.fillAmount = (timeRun * 0.9f / perStep) / toTimeWait;
            }
            else
            {
                imgFill.fillAmount = 0.9f + 0.1f*(timeRun - timestep1) / (toTimeWait*(1.0f - perStep));
            }
            int nf = (int)(imgFill.fillAmount * 100);
            if (nf > 100)
            {
                nf = 100;
            }
            txtFill.text = strFill + " " + nf + "%";

            imgWatermelonRectTransform.anchoredPosition = Vector2.Lerp(watermelonStartPos, watermelonEndPos, imgFill.fillAmount);
            float rotationAngle = imgFill.fillAmount  *360f*  3; // Adjust 3 as needed for speed
            imgWatermelonRectTransform.localRotation = Quaternion.Euler(0, 0, -rotationAngle);

            if (timeRun >= toTimeWait)
            {
                txtFill.text = strFill + " 100%";
                // SDKManager.Instance.isAllowShowFirstOpen = true;
                // SDKManager.Instance.onSplashFinishLoding();
                GameManager.Instance.Ready();
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}