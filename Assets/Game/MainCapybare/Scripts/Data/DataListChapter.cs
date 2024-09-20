using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capybara
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(DataListChapter))]
    public class DataListChapterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DataListChapter dataListChapter = (DataListChapter)target;

            if (GUILayout.Button("Reset All Chapters"))
            {
                ResetChapters(dataListChapter);
            }
        }

        private void ResetChapters(DataListChapter dataListChapter)
        {
            Undo.RecordObject(dataListChapter, "Reset All Chapters");

            foreach (Chapter chapter in dataListChapter.chapter)
            {
                chapter.isUnlocked = false;
            }

            EditorUtility.SetDirty(dataListChapter);
            AssetDatabase.SaveAssets();

            Debug.Log("All chapter Unlocked.");
        }
    }
#endif
    [CreateAssetMenu(fileName = "ListChapter", menuName = "Data/Capybara/ListChapter")]
    public class DataListChapter : ScriptableObject
    {
        public List<Chapter> chapter = new List<Chapter>();
    }

    [System.Serializable]
    public class Chapter
    {
        public string chapterName;
        public bool isUnlocked;
        public GameObject chapterPrefab;
        public DataChapter dataChapter;
    }
}
