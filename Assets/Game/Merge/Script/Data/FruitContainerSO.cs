using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(FruitContainerSO))]
public class FruitContainerSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // if (GUILayout.Button("Generate"))
        // {
        //     FruitContainerSO fruitContainer = target as FruitContainerSO;
        //     fruitContainer.container = new FruitInfo[fruitContainer.fruits.Length];
        //     for (int i = 0; i < fruitContainer.fruits.Length; i++)
        //     {
        //         fruitContainer.container[i].assetPath = fruitContainer.path + fruitContainer.fruits[i].gameObject.name;
        //         fruitContainer.container[i].fruitType = fruitContainer.fruits[i].GetFruitType();
        //     }
        //     EditorUtility.SetDirty(target);
        // }
    }
}
#endif

[CreateAssetMenu(fileName = "FruitContainer", menuName = "Data/FruitContainer")]
public class FruitContainerSO : ScriptableObject
{
    public FruitInfo[] container;
    // public Fruit[] fruits;
    // public string path;
    public FruitInfo GetFruit(FruitType fruitType)
    {
        for (int i = 0; i < container.Length; i++)
        {
            if (container[i].fruitType == fruitType)
            {
                return container[i];
            }
        }
        return default;
    }
}
[System.Serializable]
public struct FruitInfo
{
    public string assetPath;
    public FruitType fruitType;
    public Color color;
    public float scale;
    public Fruit GetObject()
    {
        return Resources.Load<Fruit>(assetPath);
    }
}