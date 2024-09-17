using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "FruitItemObject", menuName = "Data/Inventory/FruitItemObject")]
public class FruitItemObjectSO : ItemObjectSO
{
    public FruitType fruitType;
    public Color color;
    public float fxScale;
    public FruitSkinData[] fruitSkins;
}
[System.Serializable]
public struct FruitSkinData
{
    public Sprite fruitSkin;
    public float spriteScale;
    public Color fxColor;
}
