using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Capybara
{
    public class BuildListUI : MonoBehaviour
    {
        [SerializeField] private BuildList buildList;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform buttonContainer;
        [SerializeField] private GameObject ChapterPrefab;
        [SerializeField] private int currentChapter = 1;

        private Dictionary<Button, GameObject> buttonToPrefabMap = new Dictionary<Button, GameObject>();

        private void Start()
        {
            LoadBuildListForChapter(currentChapter);
        }

        public void SetChapter(int chapter)
        {
            currentChapter = chapter;
            ClearExistingButtons();
            LoadBuildListForChapter(currentChapter);
        }

        private void LoadBuildListForChapter(int chapter)
        {
            if (buildList == null)
            {
                Debug.LogError("BuildList is not assigned!");
                return;
            }

            if (chapter <= 0 || chapter > buildList.buildPrefabHierarchies.Count)
            {
                Debug.LogError($"Invalid chapter number: {chapter}");
                return;
            }

            var hierarchy = buildList.buildPrefabHierarchies[chapter - 1];
            GameObject parentInstance = Instantiate(hierarchy.parentPrefab, ChapterPrefab.transform);
            parentInstance.name = hierarchy.parentPrefab.name;

            foreach (var childPrefabState in hierarchy.childPrefabStates)
            {
                FindChildInParent(parentInstance, childPrefabState.childPrefab.name).SetActive(childPrefabState.isActive);
                CreateBuildButton(childPrefabState, parentInstance);
            }
            hierarchy.IsBuild = true;
        }

        private void CreateBuildButton(ChildPrefabState childPrefabState, GameObject parentInstance)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            GameObject childPrefab = childPrefabState.childPrefab;
            if (buttonText != null)
            {
                buttonText.text = childPrefab.name;
            }

            GameObject childInstance = FindChildInParent(parentInstance, childPrefab.name);
            if (childInstance != null)
            {
                buttonToPrefabMap[button] = childInstance;
                button.onClick.AddListener(() => 
                {
                    childPrefabState.isActive = !childPrefabState.isActive;
                    childInstance.SetActive(childPrefabState.isActive);
                });
            }
            else
            {
                Debug.LogWarning($"Child prefab {childPrefabState.childPrefab.name} not found in parent instance.");
            }
        }

        private GameObject FindChildInParent(GameObject parent, string childName)
        {
            Transform childTransform = parent.transform.Find(childName);
            return childTransform != null ? childTransform.gameObject : null;
        }

        private void ClearExistingButtons()
        {
            foreach (Transform child in buttonContainer)
            {
                Destroy(child.gameObject);
            }
            buttonToPrefabMap.Clear();
        }
    }
}