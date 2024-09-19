using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capybara
{
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
