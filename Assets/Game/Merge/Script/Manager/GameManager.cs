using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using Unity.VisualScripting;
using UnityEngine.PlayerLoop;
using System.Data;
namespace Merge
{
    public class GameManager : Singleton<GameManager>
    {
        public static EGameState GameState { get; private set; }
        public static EGameMode GameMode { get; private set; }
        public DataManager dataManager;
        public UIManager UIManager;
        public HeartManager heartManager;
        public GameMode currentMode;
        public int currentTheme;
        public static event Action<float> PassMinutes;
        public static event Action OnPause;
        public static event Action OnResume;
        private float timer;
        public static int[] CollapseSystem { get; private set; }
        public static int[] NewFruitSystem { get; private set; }
        public static int CurrentLevel { get; internal set; }

        public static bool isOnEditMode;
        public static bool isOnLoadLevel;
        public Transform levelItemsContainer;

        private void Start()
        {
            // SDKManager.CBFinishloadDing += Ready;
            Application.targetFrameRate = 60;
            Initialize();
        }
        public void Initialize()
        {
            dataManager.Initialize();
            UIManager.Initialize();
            timer = 0;
        }
        public void Pause()
        {
            Time.timeScale = 0;
            SwitchGameState(EGameState.PAUSE);
            OnPause?.Invoke();
        }
        public void Resume()
        {
            Time.timeScale = 1;
            SwitchGameState(EGameState.RUNNING);
            OnResume?.Invoke();
        }
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 60)
            {
                timer = 0;
                DataManager.TotalMinutePlay++;
                GameRes.IncreaseLevel();
                PassMinutes?.Invoke(DataManager.TotalMinutePlay);
            }
        }
        public void Ready()
        {
            // API_GetAccessToken.Instance.Request(d =>
            // {
            //     if (d != null && d.success)
            //     {
            //         DataManager.access_token = d.data.access_token;
            //     }

            // }, new DataUser()
            // {
            //     password = "MergeFruitLog@2024",
            //     player_uuid = DataManager.UserId
            // });
            // AdsHelperWrapper.SetBannerShowState(true);
            bool isFirstTime = PlayerPrefs.GetInt("IsFirstTime", 1) == 1;
            string da = PlayerPrefsUtil.CF_CollapseBanner;
            string[] arx = da.Split(",");
            CollapseSystem = new int[arx.Length];
            for (int i = 0; i < arx.Length; i++)
            {
                CollapseSystem[i] = Convert.ToInt32(arx[i]);
            }
            da = PlayerPrefsUtil.CF_NewFruitMerge;
            arx = da.Split(",");
            NewFruitSystem = new int[arx.Length];
            for (int i = 0; i < arx.Length; i++)
            {
                NewFruitSystem[i] = Convert.ToInt32(arx[i]);
            }
            BackToMenu();
            UIStartTutorial uIStartTutorial = UIManager.Instance.ShowPopup<UIStartTutorial>(null);
            // uIStartTutorial.txtTutorial.SetActive(true);
            // if (isFirstTime)
            // {
            //     UIStartTutorial uIStartTutorial = UIManager.Instance.ShowPopup<UIStartTutorial>(null);
            //     PlayerPrefs.SetInt("IsFirstTime", 0);
            //     PlayerPrefs.Save();
            // }
            // if (MGTime.Instance.IsNewDay(DataManager.Instance.userData.lastTimeLogin))
            // {
            //     DataManager.Instance.userData.lastTimeLogin = MGTime.Instance.GetUtcTime();
            //     if (DataManager.Instance.userData.firstTimeJoinGame <= 0)
            //     {
            //         DataManager.Instance.userData.firstTimeJoinGame = MGTime.Instance.GetUtcTime();
            //     }

            //     var dayInstall = MGTime.TimestampToDateTime(MGTime.Instance.GetUtcTime()).Date - MGTime.TimestampToDateTime(DataManager.Instance.userData.firstTimeJoinGame).Date;
            //     DataManager.Instance.userData.day_install = dayInstall.Days + 1;
            //     DataManager.Instance.userData.day_login++;
            //     DataManager.Instance.SaveData();
            // }
            // LogEventHub.Log(LogEvent.game_open);

        }

        public void ClearCurrentMode()
        {
            if (currentMode)
            {
                currentMode.Clear();
                Destroy(currentMode.gameObject);
            }
        }

        public void PlayClassicMode()
        {
            isOnEditMode = false;
            ClearCurrentMode();
            AudioManager.Instance.PlayMusic("BGM", 0.7f, true);
            SwitchGameState(EGameState.RUNNING);
            ChangeGameMode(EGameMode.CLASSIC);
            currentMode = Resources.Load<GameMode>("GameMode/ClassicMode");
            currentMode = Instantiate(currentMode, levelItemsContainer);
            currentMode.Initialize();
            currentMode.LoadLevel();
            Time.timeScale = 1;
        }

        public void PlayAdventureMode()
        {
            isOnEditMode = false;
            ClearCurrentMode();
            AudioManager.Instance.PlayMusic("BGM", 0.7f, true);
            ChangeGameMode(EGameMode.ADVENTURE);
            SwitchGameState(EGameState.RUNNING);
            currentMode = Resources.Load<GameMode>("GameMode/AdventureMode");
            currentMode = Instantiate(currentMode, transform);
            currentMode.Initialize();
            currentMode.LoadLevel();
            Time.timeScale = 1;
        }
        public void PlayEditMode()
        {
            isOnEditMode = true;
            ClearCurrentMode();
            ChangeGameMode(EGameMode.ADVENTURE);
            SwitchGameState(EGameState.RUNNING);
            currentMode = Resources.Load<GameMode>("GameMode/AdventureMode");
            currentMode = Instantiate(currentMode, transform);
            currentMode.Initialize();
            currentMode.LoadLevel();
        }
        public void BackToMenu()
        {
            ClearCurrentMode();
            PlayClassicMode();
            // UIManager.Instance.ShowScreen<UIMaingameScreen>();
        }
        public void SwitchGameState(EGameState newState)
        {
            GameState = newState;
        }
        public void ChangeGameMode(EGameMode newMode)
        {
            GameMode = newMode;
        }
    }

    public enum EGameState
    {
        NONE, RUNNING, PAUSE
    }
    public enum EGameMode
    {
        CLASSIC, ADVENTURE
    }
}