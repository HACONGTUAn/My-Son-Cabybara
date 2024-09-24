using Fishing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{   
    [SerializeField] private Text timerText;
    private bool iso = true;
    public int timer = 30;
    private void Awake()
    {
        
    }
   
    public void TimmerCount()
    {
        StartCoroutine(StartTimerCount());
    }
    private IEnumerator StartTimerCount()
    {
        timerText.text = timer + "";
        while (timer > 0)
        {           
            timer--;
            yield return new WaitForSeconds(1f);
            timerText.text = timer + "";
        }
        
        GameManager.Instance.SwitchGameState(GameState.SlashFish);
    }
}
