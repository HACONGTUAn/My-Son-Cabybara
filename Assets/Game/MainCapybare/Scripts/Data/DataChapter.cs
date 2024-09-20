using System;
using System.Collections.Generic;
using UnityEngine;

namespace Capybara
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomPropertyDrawer(typeof(TaskChapter))]
    public class TaskChapterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float yPos = position.y;
            float height = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            // Vẽ trường title
            Rect titleRect = new Rect(position.x, yPos, position.width, height);
            EditorGUI.PropertyField(titleRect, property.FindPropertyRelative("taskName"), new GUIContent("Task Name"));
            yPos += height + spacing;

            // Vẽ trường isUnlocked
            Rect unlockedRect = new Rect(position.x, yPos, position.width, height);
            EditorGUI.PropertyField(unlockedRect, property.FindPropertyRelative("isUnlocked"), new GUIContent("Is Unlocked"));
            yPos += height + spacing;

            // Vẽ trường TaskType
            Rect typeRect = new Rect(position.x, yPos, position.width, height);
            EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("type"), new GUIContent("Task Type"));
            yPos += height + spacing;

            // Lấy giá trị hiện tại của TaskType
            var taskType = (TaskChapter.TaskType)property.FindPropertyRelative("type").enumValueIndex;

            // Vẽ các trường khác dựa trên TaskType
            Rect fieldRect = new Rect(position.x, yPos, position.width, height);

            switch (taskType)
            {
                case TaskChapter.TaskType.Heart:
                    EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("price"), new GUIContent("Price"));
                    break;
                case TaskChapter.TaskType.Jump:
                    EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("height"), new GUIContent("Height"));
                    break;
                case TaskChapter.TaskType.Merge:
                    EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("mergeType"), new GUIContent("Merge Type"));
                    break;
                case TaskChapter.TaskType.Fishing:
                    EditorGUI.PropertyField(fieldRect, property.FindPropertyRelative("fishingPrefab"), new GUIContent("Fishing Prefab"));
                    break;
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing * 3;
        }
    }
    [CustomEditor(typeof(DataChapter))]
    public class DataChapterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DataChapter dataChapter = (DataChapter)target;

            if (GUILayout.Button("Reset All Tasks"))
            {
                ResetTasks(dataChapter);
            }
        }

        private void ResetTasks(DataChapter dataChapter)
        {
            Undo.RecordObject(dataChapter, "Reset All Tasks");
            foreach (ListTaskChapter listTasks in dataChapter.listTasks)
            {
                foreach (TaskChapter tasks in listTasks.tasks)
                {
                    tasks.isUnlocked = false;
                }
            }

            EditorUtility.SetDirty(dataChapter);
            AssetDatabase.SaveAssets();

            Debug.Log("All Tasks Unlocked.");
        }
    }
    
#endif

    [CreateAssetMenu(fileName = "Chapter", menuName = "Data/Capybara/Chapter")]
    public class DataChapter : ScriptableObject
    {
        public List<ListTaskChapter> listTasks = new List<ListTaskChapter>();
    }
    [System.Serializable]
    public class ListTaskChapter
    {
        public List<TaskChapter> tasks = new List<TaskChapter>();
    }

    [System.Serializable]
    public class TaskChapter
    {
        public string taskName;
        public bool isUnlocked;

        public TaskType type;
        
        [ShowInInspector(TaskType.Heart)]
        public int price;

        [ShowInInspector(TaskType.Jump)]
        public int height;

        [ShowInInspector(TaskType.Merge)]
        public int mergeType;

        [ShowInInspector(TaskType.Fishing)]
        public GameObject fishingPrefab;

        public enum TaskType
        {
            Heart,
            Jump,
            Merge,
            Fishing
        }
    }

    public class ShowInInspectorAttribute : PropertyAttribute
    {
        public TaskChapter.TaskType TaskType { get; private set; }

        public ShowInInspectorAttribute(TaskChapter.TaskType taskType)
        {
            TaskType = taskType;
        }
    }
}