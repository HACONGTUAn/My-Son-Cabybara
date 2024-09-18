using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Merge
{
    public class LevelController : MonoBehaviour
    {
        public string levelFilePath = "Levels/Level1";
        public List<GameObject> itemPrefabs; 
        public string outputFilePath = "Assets/Resources/Levels/ExportedLevel.json";
        public int level;
        public MissionRequire[] missionRequires;
        public GameObject enviroment;

        void Start()
        {
            LoadLevel(levelFilePath);
        }

        public void LoadLevel(string filePath)
        {
            TextAsset jsonTextAsset = Resources.Load<TextAsset>(filePath);
            if (jsonTextAsset != null)
            {
                LevelData levelData = JsonConvert.DeserializeObject<LevelData>(jsonTextAsset.text);
                foreach (Items itemData in levelData.items)
                {
                    GameObject prefab = GetPrefabByItemType(itemData.itemType);
                    if (prefab != null)
                    {
                        GameObject instance = Instantiate(prefab, itemData.position, Quaternion.identity);
                        instance.name = itemData.itemType.ToString() + itemData.itemID;
                    }
                    else
                    {
                        Debug.LogError($"Prefab not found for item type: {itemData.itemType}");
                    }
                }
            }
            else
            {
                Debug.LogError($"Level file not found at path: {filePath}");
            }
        }

        private GameObject GetPrefabByItemType(ItemType itemType)
        {
            // Giả sử danh sách prefab được sắp xếp theo thứ tự của ItemType
            int index = (int)itemType;
            if (index >= 0 && index < itemPrefabs.Count)
            {
                return itemPrefabs[index];
            }
            return null;
        }

        public void ExportLevel()
        {
            List<Items> items = new List<Items>();

            foreach (GameObject prefab in itemPrefabs)
            {
                ItemType itemType = GetItemTypeFromPrefab(prefab);
                GameObject[] instances = GameObject.FindGameObjectsWithTag(prefab.tag);
                foreach (GameObject instance in instances)
                {
                    Items itemData = new Items
                    {
                        itemID = instance.GetInstanceID(),
                        itemType = itemType,
                        position = instance.transform.position
                    };
                    items.Add(itemData);
                }
            }
            for (int i = 2; i < enviroment.transform.childCount; i++)
            {
                GameObject child = enviroment.transform.GetChild(i).gameObject;

            }

            LevelData levelData = new LevelData
            {
                level = level,
                missionRequires = missionRequires,
                items = items
            };

            string json = JsonConvert.SerializeObject(levelData, Formatting.Indented);
            File.WriteAllText(outputFilePath, json);

            Debug.Log($"Level exported to {outputFilePath}");
        }

        private ItemType GetItemTypeFromPrefab(GameObject prefab)
        {     
            int index = itemPrefabs.IndexOf(prefab);
            if (index >= 0 && index < itemPrefabs.Count)
            {
                return (ItemType)index;
            }
            return ItemType.Fruit; 
        }
    }


    [System.Serializable]
    public class LevelData
    {
        public LevelData() 
        {
            items = new List<Items>();
        }
        public LevelData(LevelData levelData)
        {
            this.level = levelData.level;
            this.moveAmount = levelData.moveAmount;
            this.missionRequires = levelData.missionRequires;
            this.items = new List<Items>(levelData.items);
            this.cups = new List<CupData>(levelData.cups);
            this.fruits = new List<FruitLVData>(levelData.fruits);
            this.chainData = levelData.chainData; 
            this.timeCount = levelData.timeCount;
            this.timeRemaining = levelData.timeRemaining;
            this.hasMoveCount = levelData.hasMoveCount;
            this.hasScoreCount = levelData.hasScoreCount;
            this.scoreRemaining = levelData.scoreRemaining; 
        }
        public int level;
        public bool hasMoveCount;
        public int moveAmount;  
        public MissionRequire[] missionRequires;
        public List<Items> items;
        public List<CupData> cups;
        public List<FruitLVData> fruits;
        public ChainData chainData;
        public bool timeCount;
        public int timeRemaining;
        public bool hasScoreCount;
        public int scoreRemaining;  
    }

    [System.Serializable]
    public class MissionRequire
    {
        public int itemID;
        public ItemType itemType;
        public int requireAmount;
    }

    [System.Serializable]
    public class Items
    {
        public int itemID;
        public ItemType itemType;
        public Vector3 position;
        public bool inIce;
    }
    [Serializable]
    public class CupData
    {
        public int itemID; 
        public FruitType fruitType;
        public Vector3 position;
        public bool inIce;
    }
    [Serializable]
    public class ChainData
    {
        public bool hasChain;
        public int itemID;
        public FruitType fruitType;
        public Vector3 position;
    }
    [Serializable]
    public class FruitLVData
    {
        public FruitType fruitType;
        public Vector3 position;
        public bool inIce;
    }

}