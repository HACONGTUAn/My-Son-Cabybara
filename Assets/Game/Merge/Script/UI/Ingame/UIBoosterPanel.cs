using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBoosterPanel : MonoBehaviour
{
    public const string FinishBoosterKey = "FINISH_BOOSTER";
    public const string StartUseBoosterKey = "START_BOOSTER";
    public const string CancerUseBoosterKey = "CANCER_BOOSTER";
    public const string RefreshUseBoosterKey = "REFRESH_BOOSTER";
    [SerializeField] UIBoosterConfirmPanel confirmPanel;
    [SerializeField] ClassicMode classicMode;
    [SerializeField] UIBoosterButton[] boosterButtons;
    private event Action complete;
    public void Initialize()
    {
        for (int i = 0; i < boosterButtons.Length; i++)
        {
            boosterButtons[i].Initialize(this);
        }
        Observer.AddObserver(FinishBoosterKey, Finish);
        Observer.AddObserver(CancerUseBoosterKey, Cancer);
        Observer.AddObserver(RefreshUseBoosterKey, RefreshCount);
        Refresh();
    }

    private void Cancer(object data)
    {
        SetStatusBoosters(false);
    }

    private void Finish(object data)
    {
        if (data == null) return;
        complete?.Invoke();
        complete = null;
        SetStatusBoosters(false);
    }
    private void RefreshCount(object data)
    {
        for (int i = 0; i < boosterButtons.Length; i++)
        {
            boosterButtons[i].Refresh();
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < boosterButtons.Length; i++)
        {
            boosterButtons[i].Refresh();
        }
    }
    public void UseBooster(EBoosterType boosterType, Action complete)
    {
        switch (boosterType)
        {
            case EBoosterType.REMOVE:
                Bomb();
                break;
            case EBoosterType.EVOLUTION:
                LevelUp();
                break;
            case EBoosterType.CLEAR:
                ClearFruit();
                break;
            case EBoosterType.DESTROYHORIZONTAL:
                DestroyHorizontal();
                break;
            case EBoosterType.DESTROYVERTICAL:
                DestroyVertical();
                break;
            case EBoosterType.REROLL:
                Reroll();
                break;
            case EBoosterType.CREMOVE:
                ClassicRemove();
                break;

        }
        SetStatusBoosters(true);

        Observer.Notify(StartUseBoosterKey, boosterType);
        this.complete = complete;
    }


    private void DestroyHorizontal()
    {
        GameManager.Instance.currentMode.GetComponent<ClassicMode>().DestroyHorizontalFruits();
    }
    private void DestroyVertical()
    {
        GameManager.Instance.currentMode.GetComponent<ClassicMode>().DestroyVerticalFruits();
    }
    private void Reroll() 
    {
        // GameManager.Instance.currentMode.GetComponent<ClassicMode>().Reroll();
    }
    private void SetStatusBoosters(bool status)
    {
        for (int i = 0; i < boosterButtons.Length; i++)
        {
            boosterButtons[i].SetActiveCancer(status);
        }
    }
    private void LevelUp()
    {
        GameManager.Instance.currentMode.GetComponent<ClassicMode>().EvolveFruit();
    }

    private void ClearFruit()
    {
        confirmPanel.Active("Do you want to clear all fruit below tomato", (a) =>
        {
            if (a)
            {
                GameManager.Instance.currentMode.GetComponent<ClassicMode>().ClearFruits();
            }
            else
            {
                Observer.Notify(CancerUseBoosterKey);
            }
        });
    }

    private void Bomb()
    {
        if (GameManager.Instance.currentMode.GetComponent<ClassicMode>())
        {
            GameManager.Instance.currentMode.GetComponent<ClassicMode>().SelectRemove();
        }    
    }

    private void ClassicRemove()
    {
        if (GameManager.Instance.currentMode.GetComponent<ClassicMode>())
        {
            GameManager.Instance.currentMode.GetComponent<ClassicMode>().SelectRemove();
        }
    }

    public void Dispose()
    {
        Observer.RemoveObserver(FinishBoosterKey, Finish);
    }
}
public enum EBoosterType
{
    REMOVE, DESTROYHORIZONTAL, DESTROYVERTICAL, REROLL, CREMOVE, CLEAR, EVOLUTION, CUT
}