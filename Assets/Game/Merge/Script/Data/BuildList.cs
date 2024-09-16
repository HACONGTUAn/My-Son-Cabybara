using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Capybara
{
    [System.Serializable]
    public class ChildPrefabState
    {
        public GameObject childPrefab;
        public bool isActive = false;
    }

    [System.Serializable]
    public class PrefabHierarchy
    {
        public bool IsBuild;
        public GameObject parentPrefab;
        public List<ChildPrefabState> childPrefabStates = new List<ChildPrefabState>();

        public void UpdateChildStates()
        {
            if (!IsBuild)
            {
                foreach (var childState in childPrefabStates)
                {
                    childState.isActive = false;
                }
            }
        }
    }

    [CreateAssetMenu(fileName = "BuildList", menuName = "Capybara/AllBuilds")]
    public class BuildList : ScriptableObject
    {
        public List<PrefabHierarchy> buildPrefabHierarchies = new List<PrefabHierarchy>();

        public const string ResourcePath = "Assets/Game/Merge/Resources/Data";

        #if UNITY_EDITOR
        private static bool isSyncing = false;

        private void OnEnable()
        {
            EditorApplication.delayCall += DelayedSync;
        }

        private void OnDisable()
        {
            EditorApplication.delayCall -= DelayedSync;
        }

        private void DelayedSync()
        {
            SyncBuildPrefabs();
        }

        public void SyncBuildPrefabs()
        {
            if (isSyncing) return;

            isSyncing = true;
            try
            {
                var newBuildPrefabHierarchies = LoadBuildPrefabsFromFolder("Build");               
                newBuildPrefabHierarchies = SortBuildList(newBuildPrefabHierarchies);

                // Preserve IsBuild values and child prefab states
                foreach (var newHierarchy in newBuildPrefabHierarchies)
                {
                    var existingHierarchy = buildPrefabHierarchies.FirstOrDefault(h => h.parentPrefab == newHierarchy.parentPrefab);
                    if (existingHierarchy != null)
                    {
                        newHierarchy.IsBuild = existingHierarchy.IsBuild;
                        
                        if (newHierarchy.IsBuild)
                        {
                            foreach (var newChildState in newHierarchy.childPrefabStates)
                            {
                                var existingChildState = existingHierarchy.childPrefabStates.FirstOrDefault(c => c.childPrefab == newChildState.childPrefab);
                                if (existingChildState != null)
                                {
                                    newChildState.isActive = existingChildState.isActive;
                                }
                            }
                        }
                    }
                    newHierarchy.UpdateChildStates();
                }

                buildPrefabHierarchies = newBuildPrefabHierarchies;

                EditorUtility.SetDirty(this);
                EditorApplication.delayCall += () =>
                {
                    AssetDatabase.SaveAssets();
                    Debug.Log("BuildList synced and saved.");
                };
            }
            finally
            {
                isSyncing = false;
            }
        }

        private List<PrefabHierarchy> LoadBuildPrefabsFromFolder(string folderName)
        {
            string folderPath = Path.Combine(ResourcePath, folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            List<PrefabHierarchy> prefabHierarchies = new List<PrefabHierarchy>();

            foreach (string filePath in Directory.GetFiles(folderPath, "*.prefab"))
            {
                GameObject mainPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
                if (mainPrefab != null)
                {
                    PrefabHierarchy hierarchy = new PrefabHierarchy
                    {
                        parentPrefab = mainPrefab,
                        childPrefabStates = GetChildPrefabStates(mainPrefab)
                    };
                    prefabHierarchies.Add(hierarchy);
                }
            }

            return prefabHierarchies;
        }

        private List<ChildPrefabState> GetChildPrefabStates(GameObject parentPrefab)
        {
            List<ChildPrefabState> childPrefabStates = new List<ChildPrefabState>();

            foreach (Transform child in parentPrefab.transform)
            {
                if (child != null)
                {
                    childPrefabStates.Add(new ChildPrefabState { childPrefab = child.gameObject, isActive = false });
                }
            }

            return childPrefabStates;
        }

        private List<PrefabHierarchy> SortBuildList(List<PrefabHierarchy> buildPrefabHierarchies)
        {
            return buildPrefabHierarchies.OrderBy(p => int.Parse(p.parentPrefab.name.Split(' ')[1])).ToList();
        }
        #endif
    }

    #if UNITY_EDITOR
    public class BuildListPostprocessor : AssetPostprocessor
    {
        private static bool isProcessing = false;

        private static void OnPostprocessAllAssets(
            string[] importedAssets, 
            string[] deletedAssets, 
            string[] movedAssets, 
            string[] movedFromAssetPaths)
        {
            if (isProcessing) return;

            isProcessing = true;
            try
            {
                if (ShouldSyncBuildList(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths))
                {
                    EditorApplication.delayCall += SyncBuildList;
                }
            }
            finally
            {
                isProcessing = false;
            }
        }

        private static bool ShouldSyncBuildList(params string[][] assetArrays)
        {
            return assetArrays.SelectMany(x => x)
                .Any(path => path.StartsWith(BuildList.ResourcePath) && 
                             path.EndsWith(".prefab") && 
                             !path.EndsWith("BuildList.asset"));
        }

        private static void SyncBuildList()
        {
            string BuildListPath = $"{BuildList.ResourcePath}/BuildList.asset";
            var buildList = AssetDatabase.LoadAssetAtPath<BuildList>(BuildListPath);
            if (buildList != null)
            {
                buildList.SyncBuildPrefabs();
            }
            else
            {
                Debug.LogWarning($"BuildList not found at {BuildListPath}. Please create one.");
            }
        }
    }
    #endif
}