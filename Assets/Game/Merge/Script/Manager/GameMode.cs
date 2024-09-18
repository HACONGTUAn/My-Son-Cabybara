using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Merge
{
    public abstract class GameMode : MonoBehaviour
    {
        public bool isActive;
        public abstract void Initialize();
        public abstract void LoadLevel();
        public abstract void Clear();
    }
}
