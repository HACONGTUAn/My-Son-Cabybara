using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraJump
{
    [CreateAssetMenu(fileName = "Booster", menuName = "Capybara/Jump/Booster")]
    public class BoosterSO : ScriptableObject
    {
        [Serializable]
        public class BoosterEntry
        {
            public BoosterType type;
            public int price;
        }

        public List<BoosterEntry> boosterPrices = new List<BoosterEntry>();


        public int GetBoosterPrice(BoosterType type)
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