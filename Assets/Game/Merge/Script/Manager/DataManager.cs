using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using mygame.sdk;

namespace EditorClass
{

#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(DataManager))]
    public class DataManagerEditor : Editor
    {
        public Dictionary<string, string> prefInfos;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (prefInfos == null)
            {
                foldOut = true;
                if (GUILayout.Button("Show Prefs"))
                {
                    string data = PlayerPrefs.GetString("user_data", "");
                    if (data == null || data.Length == 0) return;
                    var userData = JsonConvert.DeserializeObject<PlayerData>(data);
                    if (userData.prefData == null || userData.prefData.Length == 0) return;
                    prefInfos = JsonConvert.DeserializeObject<Dictionary<string, string>>(userData.prefData);
                }
            }
            else
            {
                ShowPrefs();
                if (GUILayout.Button("Save Prefs"))
                {
                    string data = PlayerPrefs.GetString("user_data", "");
                    if (data == null || data.Length == 0) return;
                    var userData = JsonConvert.DeserializeObject<PlayerData>(data);
                    string newData = JsonConvert.SerializeObject(prefInfos);
                    userData.prefData = newData;
                    var d = JsonConvert.SerializeObject(userData);
                    PlayerPrefs.SetString("user_data", d);
                    Debug.Log("Saved:" + d);
                    if (Application.isPlaying)
                    {
                        DataManager dataManager = target as DataManager;
                        dataManager.userData.prefData = newData;
                        dataManager.SendMessage("ReloadPref");
                    }
                }
            }
        }

        private bool foldOut;
        private void ShowPrefs()
        {
            int index = 0;
            foldOut = EditorGUILayout.Foldout(foldOut, "ListPrefs");
            if (foldOut)
            {
                EditorGUI.indentLevel++;
                var newList = new Dictionary<string, string>(prefInfos);
                foreach (var item in newList)
                {
                    EditorGUILayout.LabelField("Element " + index);
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(item.Key);
                    string newValue = "";
                    if (int.TryParse(item.Value, out int newNum))
                    {
                        newValue = EditorGUILayout.IntField(newNum).ToString();
                    }
                    else
                    {
                        newValue = EditorGUILayout.TextField(item.Value);
                    }
                    if (newValue != item.Value)
                    {
                        prefInfos[item.Key] = newValue;
                        Debug.Log("change " + item.Key + ":" + newValue);
                    }
                    EditorGUILayout.EndHorizontal();
                    index++;
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }

        }
    }
#endif
}
public class DataManager : Singleton<DataManager>
{
    public static event Action<int> OnAddHeart;
    public static event Action<int> OnAddCoin;
    private string userDataPref
    {
        get { return PlayerPrefs.GetString("user_data", ""); }
        set { PlayerPrefs.SetString("user_data", value); }
    }
    public static int HighScoreClassicMode
    {
        get { return PlayerPrefs.GetInt("classic_mode_high_score", 0); }
        set { PlayerPrefs.SetInt("classic_mode_high_score", value); }
    }
    public static int Level
    {
        get { return GameRes.GetLevel(Level_type.Adventure); }
        set
        {
            GameRes.SetLevel(Level_type.Adventure, value);
        }
    }
    public int lastLeagueScoreDisplay
    {
        get { return PlayerPrefs.GetInt("lastLeagueScoreDisplay", 0); }
        set { PlayerPrefs.SetInt("lastLeagueScoreDisplay", value); }
    }
    public static int Heart
    {
        get => PlayerPrefs.GetInt("user_Heart", 5);
        set
        {
            PlayerPrefs.SetInt("user_Heart", value);
            OnAddHeart?.Invoke(value);
        }
    }

    public static int Coin
    {
        get => PlayerPrefs.GetInt("user_Coin", 0);
        set
        {
            PlayerPrefs.SetInt("user_Coin", value);
            OnAddCoin?.Invoke(value);
        }
    }

    public static float TotalMinutePlay
    {
        get { return PlayerPrefs.GetFloat("total_minute", 0); }
        set { PlayerPrefs.SetFloat("total_minute", value); }
    }
    private static Dictionary<string, string> userDicDataPref;
    public ItemObjectContainerSO itemObjectContainer;
    public PlayerData userData;
    internal static string access_token
    {
        get => PlayerPrefs.GetString("access_token", "");
        set => PlayerPrefs.SetString("access_token", value);
    }
    public static int DayInstall
    {
        get => PlayerPrefs.GetInt("day_install", 0);
        set => PlayerPrefs.SetInt("day_install", value);
    }
    public static int DayLogin
    {
        get => PlayerPrefs.GetInt("day_login", 0);
        set => PlayerPrefs.SetInt("day_login", value);
    }
    public static int SectionLogin
    {
        get => PlayerPrefs.GetInt("section_login", 0);
        set => PlayerPrefs.SetInt("section_login", value);
    }
    /*private static DataUser dataUser;
    public static DataUser DataUser
    {
        get
        {
            if (dataUser == null)
            {
                dataUser = JsonConvert.DeserializeObject<DataUser>(PlayerPrefsBase.Instance().getString("data_user", ""));
                if (dataUser == null)
                {
                    dataUser = new DataUser();
                }
            }
            return dataUser;
        }
        set
        {
            dataUser = value;
            PlayerPrefsBase.Instance().getString("data_user", JsonConvert.SerializeObject(dataUser));
        }
    }*/
    public static string UserId
    {
        get
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("user_id", "")))
            {
                PlayerPrefs.SetString("user_id", getNewUserId()); 
            }
            return PlayerPrefs.GetString("user_id");    
        }
    }
    
    private static string getNewUserName()
    {
        return $"User#{UnityEngine.Random.Range(0, 10000000):0000}";
    }
    private static string getNewUserId()
    {
        // return $"{AppConfig.platformName.ToLower()}_{Guid.NewGuid()}";
        return null;
    }
    public void Initialize()
    {

    }
    public void IncreaseLevel()
    {
        GameRes.IncreaseLevel(Level_type.Adventure);
    }
    public void SaveData()
    {
        userData.prefData = JsonConvert.SerializeObject(userDicDataPref);
        string data = JsonConvert.SerializeObject(userData);
        userDataPref = data;
    }
    #region Pref
    // Don't need to mind this one 
    private void ReloadPref()
    {
        Debug.Log("Reload");
        userDicDataPref = JsonConvert.DeserializeObject<Dictionary<string, string>>(userData.prefData);
    }
    public static string GetString(string key, string defaultValue)
    {
        if (userDicDataPref == null) return defaultValue;
        if (userDicDataPref.ContainsKey(key))
        {
            return userDicDataPref[key];
        }
        else
        {
            return defaultValue;
        }
    }

    public static void SetString(string key, string newValue)
    {
        if (userDicDataPref == null) return;
        if (userDicDataPref.ContainsKey(key))
        {
            userDicDataPref[key] = newValue;
        }
        else
        {
            userDicDataPref.Add(key, newValue);
        }
        Instance.SaveData();
    }

    public static int GetInt(string key, int defaultValue)
    {
        if (userDicDataPref == null) return defaultValue;
        if (userDicDataPref.ContainsKey(key))
        {
            int data = Convert.ToInt32(userDicDataPref[key]);
            return data;
        }
        else
        {
            return defaultValue;
        }
    }
    public static void SetInt(string key, int newValue)
    {
        if (userDicDataPref == null) return;
        if (userDicDataPref.ContainsKey(key))
        {
            userDicDataPref[key] = newValue.ToString();
        }
        else
        {
            userDicDataPref.Add(key, newValue.ToString());
        }
        Instance.SaveData();
    }
    public static void DecreaseHeart(int amount)
    {
        int currentHeart = Heart;
        if (currentHeart >= amount)
        {
            Heart = currentHeart - amount;
        }
        else
        {
            Heart = 0;
        }
        Debug.Log($"Heart decreased: {currentHeart} -> {Heart}");
        OnAddHeart?.Invoke(Heart);
    }
    public static void IncreaseHeart(int amount)
    {
        int currentHeart = Heart;
        if (currentHeart < 5)
        {
            Heart = currentHeart + amount;
        }
        if (Heart > 5)
        {
            Heart = 5;
        }
        Debug.Log($"Heart increased: {currentHeart} -> {Heart}");
        OnAddHeart?.Invoke(Heart);
    }
    public static void DecreaseCoin(int amount)
    {
        int currentCoin = Coin;
        if (currentCoin >= amount)
        {
            Coin = currentCoin - amount;
        }
        else
        {
            Coin = 0;
        }
        Debug.Log($"Coin decreased: {currentCoin} -> {Coin}");
        OnAddCoin?.Invoke(Coin);
    }
    public static void IncreaseCoin(int amount)
    {
        int currentCoin = Coin;
        Coin = currentCoin + amount;
        Debug.Log($"Coin increased: {currentCoin} -> {Coin}");
        OnAddCoin?.Invoke(Coin);
    }

    #endregion
}
[System.Serializable]
public class PlayerData
{
    public string prefData;
    public long lastTimeLogin;
    public long firstTimeJoinGame;
    public int avatar_id;
    public int day_install;
    public int day_login;
    public int section_login;
    public PlayerData()
    {
        prefData = "";
    }
}