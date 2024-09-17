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
    public class ChapterDetail
    {
        public string name;
        public bool isActive;
    }

    [CreateAssetMenu(fileName = "Chapter", menuName = "Capybara/Chapter")]
    public class Chapter : ScriptableObject
    {
        public List< ChapterDetail> buildChapterDetail = new List< ChapterDetail>();
    }
}