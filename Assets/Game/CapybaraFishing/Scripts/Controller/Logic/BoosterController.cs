using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class BoosterController : MonoBehaviour
    {

        public int GetBooster(BoosterType booster)
        {
            return PlayerPrefs.GetInt(booster.ToString(),1);
        }
        public void SetBooster(BoosterType booster, int n)
        {
           // PlayerPrefs.SetInt(booster.ToString(),n);
        }
    }
    public enum BoosterType
    {
        FishingPower, FishingTime
    }
}
