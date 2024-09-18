using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Merge
{
    public abstract class ItemObjectSO : ScriptableObject
    {
        public int itemID;
        public string itemName;
        public ItemType itemType;
        public Sprite icon;
        public string assetPath;
        public T GetObject<T>()
        {
            return Resources.Load<GameObject>(assetPath).GetComponent<T>();
        }
    }
    public enum ItemType
    {
        Fruit, Object, Ice, Cup
    }

    [CreateAssetMenu(fileName = "New Ice Item", menuName = "Items/Ice Item")]
    public class IceItemObjectSO : ItemObjectSO
    {
        public GameObject inIceObject;
    }
}