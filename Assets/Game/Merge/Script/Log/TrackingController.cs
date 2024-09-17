using mygame.sdk;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Twisted;
using UnityEngine;

public class TrackingController : MonoBehaviour
{
    public static int CurrentMode => (int)GameManager.GameMode;
    public static int CurrentLevel => GameManager.CurrentLevel;
    public static int CurrentStageLevel => 0;

    private static int[] durationsPlays;
    private static int[] totalDurations;
    private static int[] requireTimes;
    private static int[] moveTotals;
    private static int[] moveRemains;
    private static int[] bonus_time;
    private static bool isCounttime = false;
    private static bool isPauseGame = false;
    private static int[] moveCounts;
    private static int[] playTime;
    public static void OnStartGame(int numOfStageLevel = 1)
    {
        durationsPlays = new int[numOfStageLevel];
        totalDurations = new int[numOfStageLevel];
        requireTimes = new int[numOfStageLevel];
        moveTotals = new int[numOfStageLevel];
        moveRemains = new int[numOfStageLevel];
        bonus_time = new int[numOfStageLevel];
        moveCounts = new int[numOfStageLevel];
        playTime = new int[numOfStageLevel];
        isPauseGame = false;
        isCounttime = false;
    }
    public static void LogMove()
    {
        moveCounts[CurrentStageLevel]++;
    }

    public static void OnPauseGame()
    {
        isPauseGame = true;
    }
    public static void OnResumeGame()
    {
        isPauseGame = false;
    }
    public static void OnBackToMenu()
    {
        isCounttime = false;
        //SetRopeRemain(GameManager.Instance.levelController.listRope);
        // LogEventHub.LogLevel(LogEvent.level_exit, CurrentLevel, 0, null, "");
        ClearTracking();
    }
    public static void OnReplay()
    {
        isCounttime = false;
        //SetRopeRemain(GameManager.Instance.levelController.listRope);
        // LogEventHub.LogLevel(LogEvent.level_retry, CurrentLevel, 0, null, "");
        ClearTracking();
    }

    public static void OnEndGame(bool isWin, string lose_by)
    {
        //SetRopeRemain(GameManager.Instance.levelController.listRope);
        isCounttime = false;
        if (isWin)
        {
            SetConsecutiveWin(1, true, CurrentMode);
            SetConsecutiveLose(0, false, CurrentMode);
            // LogEventHub.LogLevel(LogEvent.level_success, CurrentLevel, 0, null, "");
        }
        else
        {
            SetConsecutiveWin(0, false, CurrentMode);
            SetConsecutiveLose(1, true, CurrentMode);
            // LogEventHub.LogLevel(LogEvent.level_fail, CurrentLevel, 0, null, lose_by);
        }
        ClearTracking();
    }
    public static void SetRequireStageLevel(int requireTime, int totalMove)
    {
        requireTimes[CurrentStageLevel] = requireTime;
        moveTotals[CurrentStageLevel] = totalMove;
    }
    public static async void StartCountTime()
    {
        if (isCounttime) { return; }
        isCounttime = true;
        while (isCounttime)
        {
            await Task.Delay(1000);
            if (!isCounttime)
            {
                break;
            }
            else
            {
                totalDurations[CurrentStageLevel]++;
                if (!isPauseGame)
                {
                    durationsPlays[CurrentStageLevel]++;
                }
            }
        }
    }
    public static void StopCountTime()
    {
        isCounttime = false;
    }
    public static void ClearTracking()
    {
        durationsPlays = null;
        totalDurations = null;
        requireTimes = null;
        moveTotals = null;
        moveRemains = null;
        bonus_time = null;
    }

    public static int ConsecutiveWin(int mode = 0)
    {
        return PlayerPrefs.GetInt("consecutive_win_" + mode, 0);
    }
    public static int ConsecutiveLose(int mode = 0)
    {
        return PlayerPrefs.GetInt("consecutive_lose_" + mode, 0);
    }
    public static int[] GetDurationsPlay()
    {
        return durationsPlays;
    }
    public static int[] GetTotalDuration()
    {
        return totalDurations;
    }
    public static int[] GetRequireTimes()
    {
        return requireTimes;
    }


    public static int[] GetTotalMove()
    {
        return moveTotals;
    }
    public static int[] GetMoveCounts()
    {
        return moveCounts;
    }

    public static int[] GetPlayTime()
    {
        return playTime;
    }
    public static int[] GetRemainMove()
    {
        return moveRemains;
    }
    public static int[] GetBonusTime()
    {
        return bonus_time;
    }

    public static void SetBonusTime(int v)
    {
        bonus_time[CurrentStageLevel] += v;
    }
    public static void SetConsecutiveWin(int v, bool isAdd, int mode = 0)
    {
        if (isAdd)
        {
            v += ConsecutiveWin(mode);
            PlayerPrefs.SetInt("consecutive_win_" + mode, v);
        }
        else
        {
            PlayerPrefs.SetInt("consecutive_win_" + mode, v);
        }
    }
    public static void SetConsecutiveLose(int v, bool isAdd, int mode = 0)
    {
        if (isAdd)
        {
            v += ConsecutiveLose(mode);
            PlayerPrefs.SetInt("consecutive_lose_" + mode, v);
        }
        else
        {
            PlayerPrefs.SetInt("consecutive_lose_" + mode, v);
        }
    }
}
[System.Serializable]
public class DataTrackingRopeAction
{
    [DefaultValue(-1)] public int index;
    [DefaultValue(-1)] public float timeAction;
    [DefaultValue(-1)] public int countAction;
    public bool isFree;
}