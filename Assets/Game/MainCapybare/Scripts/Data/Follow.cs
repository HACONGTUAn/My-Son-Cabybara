using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capybara
{
    [CreateAssetMenu(fileName = "Follow", menuName = "Data/Capybara/Follow")]
    public class Follow : ScriptableObject
    {
        public int task;
        public int chapter;
        public DataListChapter listChapter;
    }
}
