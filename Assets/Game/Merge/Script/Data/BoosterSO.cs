using System;
using System.Collections.Generic;
using UnityEngine;

namespace Merge
{
    [CreateAssetMenu(fileName = "Booster", menuName = "Data/Booster")]
    public class BoosterSO : ScriptableObject
    {
        [Serializable]
        public class BoosterEntry
        {
            public EBoosterType type;
            public int price;
        }

        public List<BoosterEntry> boosterPrices = new List<BoosterEntry>();


        public int GetBoosterPrice(EBoosterType type)
        {
            foreach (var entry in boosterPrices)
            {
                if (entry.type == type)
                {
                    return entry.price;
                }
            }
            return -1; 
        }
    }
}