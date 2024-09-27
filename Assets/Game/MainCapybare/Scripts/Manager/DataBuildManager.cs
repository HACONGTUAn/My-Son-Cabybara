using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capybara
{
    public class DataBuildManager : Singleton<DataBuildManager>
    {
        [HideInInspector] public int mergeType { get; set; }
        [HideInInspector] public int jumpHeight{ get; set; }
        [HideInInspector] public GameObject fishingPref { get; set; }
        [HideInInspector] public Sprite sprite{ get; set; }

        public void IsCompleted(int type)
        {
            foreach (var build in BuildManager.Instance.uIBuilds)
            {
                if((int)build.Value.type == type)
                {
                    build.Value.isCompleted = true;
                    sprite = build.Key.Icon.sprite;
                }
            }
        }
    }
}
