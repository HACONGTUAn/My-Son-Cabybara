using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(ItemObjectContainerSO))]
public class ItemObjectContainerSOEditor : Editor
{
    public string prefix = "";
    public int indexSelect;
    public bool enableAutoGenerate;
    public override void OnInspectorGUI()
    {
        enableAutoGenerate = EditorGUILayout.Toggle("Enable Auto Generate", enableAutoGenerate);
        base.OnInspectorGUI();
        if (enableAutoGenerate)
        {
            ItemObjectContainerSO container = target as ItemObjectContainerSO;
            ItemObjectSO[] all = ScripableObjectExtentions.GetAllInstances<ItemObjectSO>();
            List<string> listString = new List<string>();
            listString.Add("All");
            for (int i = 0; i < all.Length; i++)
            {
                if (listString.Contains(all[i].GetType().Name)) continue;
                listString.Add(all[i].GetType().Name);
            }
            indexSelect = EditorGUILayout.Popup("Select Type", indexSelect, listString.ToArray());
            prefix = EditorGUILayout.TextField("prefixName", prefix);
            if (GUILayout.Button("Generate ID"))
            {
                for (int i = 0; i < container.container.Length; i++)
                {
                    container.container[i].itemID = i;
                    EditorUtility.SetDirty(container.container[i]);
                }
            }
            if (GUILayout.Button("ChangeNames"))
            {
                for (int i = 0; i < container.container.Length; i++)
                {
                    string nname = prefix + (i + 1).ToString();
                    string pa = AssetDatabase.GetAssetPath(container.container[i]);
                    AssetDatabase.RenameAsset(pa, nname);
                    AssetDatabase.SaveAssetIfDirty(container.container[i]);
                }
            }
            if (GUILayout.Button("Get All Items"))
            {
                string nameTypeObj = listString[indexSelect];
                List<ItemObjectSO> list = new List<ItemObjectSO>();
                for (int i = 0; i < all.Length; i++)
                {
                    if (nameTypeObj == "All")
                    {
                        list.Add(all[i]);
                    }
                    else
                    {
                        if (all[i].GetType().Name == nameTypeObj)
                        {
                            list.Add(all[i]);
                        }
                    }
                }
                container.container = list.ToArray();
                EditorUtility.SetDirty(container);
            }
        }
        if (GUILayout.Button("Create"))
        {
            ItemObjectContainerSO container = target as ItemObjectContainerSO;
            for (int i = 0; i < container.fruits.Length; i++)
            {
                FruitItemObjectSO item = ScriptableObject.CreateInstance<FruitItemObjectSO>();
                item.itemID = i + 1;
                item.itemName = "Fruit " + item.itemID;
                item.icon = container.fruits[i].GetSprite();
                item.assetPath = "Fruits/" + container.fruits[i].name;
                item.fruitType = container.fruits[i].GetFruitType();
                string path = "Assets/_Game/Data/Fruits/Fruit" + item.itemID + ".asset";
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                AssetDatabase.CreateAsset(item, path);
            }
        }
    }
}
#endif
[CreateAssetMenu(fileName = "ItemContainer", menuName = "Data/Inventory/ItemContainer")]
public class ItemObjectContainerSO : ScriptableObject
{
    public Fruit[] fruits;
    public ItemObjectSO[] container;
    public ItemObjectSO GetItemObject(int id, ItemType itemType)
    {
        for (int i = 0; i < container.Length; i++)
        {
            if (container[i].itemType != itemType) continue;
            if (container[i].itemID != id) continue;
            return container[i];
        }
        return null;
    }
    public T GetItemObject<T>(int id, ItemType itemType) where T : ItemObjectSO
    {
        for (int i = 0; i < container.Length; i++)
        {
            if (container[i].itemType != itemType) continue;
            if (container[i].itemID != id) continue;
            return container[i] as T;
        }
        return null;
    }
}
