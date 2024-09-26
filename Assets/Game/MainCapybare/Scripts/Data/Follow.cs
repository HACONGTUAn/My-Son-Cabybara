using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capybara
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(Follow))]
    public class FollowEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Follow follow = (Follow)target;

            if (GUILayout.Button("Reset All Follow"))
            {
                ResetFollow(follow);
            }
        }

        private void ResetFollow(Follow follow)
        {
            Undo.RecordObject(follow, "Reset All Follow");
            follow.task = 0;
            follow.chapter = 0;
            foreach (Chapter chapter in follow.listChapter.chapter)
            {
                chapter.isUnlocked = false;
                foreach (ListTaskChapter listTasks in chapter.dataChapter.listTasks)
                {
                    foreach (TaskChapter tasks in listTasks.tasks)
                    {
                        tasks.isUnlocked = false;
                        tasks.isCompleted = false;
                    }
                }
            }

            EditorUtility.SetDirty(follow);
            AssetDatabase.SaveAssets();

            Debug.Log("All Follow Unlocked.");
        }
    }
#endif
    [CreateAssetMenu(fileName = "Follow", menuName = "Data/Capybara/Follow")]
    public class Follow : ScriptableObject
    {
        public int task;
        public int chapter;
        public DataListChapter listChapter;
    }
}
