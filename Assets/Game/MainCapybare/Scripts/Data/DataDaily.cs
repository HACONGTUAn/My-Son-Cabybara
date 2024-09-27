using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capybara
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(DataDaily))]
    public class DataDailyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DataDaily dataDaily = (DataDaily)target;

            if (GUILayout.Button("Reset All Dailys"))
            {
                ResetDailys(dataDaily);
            }
        }

        private void ResetDailys(DataDaily dataDaily)
        {
            Undo.RecordObject(dataDaily, "Reset All Dailys");
            dataDaily.dayGame = 0;
            dataDaily.dayReal = 0;
            foreach (Daily daily in dataDaily.daily)
            {
                daily.isUnlocked = false;
            }

            EditorUtility.SetDirty(dataDaily);
            AssetDatabase.SaveAssets();

            Debug.Log("All Daily Unlocked.");
        }
    }
#endif
    [CreateAssetMenu(fileName = "Daily", menuName = "Data/Capybara/Daily")]
    public class DataDaily : ScriptableObject
    {
        public int dayGame;
        public int dayReal;
        public List<Daily> daily = new List<Daily>();
    }

    [System.Serializable]
    public class Daily
    {
        public string DailyName;
        public bool isUnlocked;
        public int ticket;
    }
}
