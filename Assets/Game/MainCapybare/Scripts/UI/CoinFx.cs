using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CoinFx : MonoBehaviour
{
    public static CoinFx Instance { get; private set; }
    public float range;
    public float moveTime;
    public float delayTime;
    public List<Sprite> icons;
    public List<Transform> endPos;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayFx(System.Action callBack, int idx, Transform PosStart)
    {
        StartCoroutine(DelayPlayFx(callBack, idx, PosStart));
    }

    IEnumerator DelayPlayFx(System.Action callBack, int idx, Transform PosStart)
    {
        yield return new WaitForSeconds(0.5f);
        //SoundManager.Instance.playSoundFx(//SoundManager.Instance.effCoinUI);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform curChild = transform.GetChild(i);
            curChild.gameObject.SetActive(true);
            curChild.GetComponent<Image>().sprite = icons[idx];
            float ranNumX = Random.Range(-range, range) + PosStart.localPosition.x;
            float ranNumY = Random.Range(-range, range) + PosStart.localPosition.y;
            Debug.Log(PosStart.localPosition.x);
            curChild.localPosition = new Vector3(ranNumX, ranNumY);
            curChild.localScale = Vector3.zero;
            curChild.DOScale(1, moveTime).SetEase(Ease.OutElastic).SetDelay(Random.Range(0, 0.3f)).OnComplete(() =>
            {
                curChild.DOMove(endPos[idx].position, moveTime).SetEase(Ease.InOutQuad).SetDelay(delayTime).OnComplete(() =>
                {
                    curChild.gameObject.SetActive(false);
                    //SoundManager.Instance.playSoundFx(//SoundManager.Instance.effCollectCoin);
                    callBack.Invoke();
                });
            });
        }
    }

}
