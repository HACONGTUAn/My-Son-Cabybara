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
        [HideInInspector] public bool Is{ get; set; }

        private void Start()
        {
            Is = PlayerPrefs.GetInt("IsCompleted") == 1;
        }
        public void IsCompleted(int type)
        {
            foreach (var build in BuildManager.Instance.uIBuilds)
            {
                if((int)build.Value.type == type)
                {
                    build.Value.isCompleted = true;
                    Is = build.Value.isCompleted;
                    PlayerPrefs.SetInt("IsCompleted", Is ? 1 : 0);
                    sprite = build.Key.Icon.sprite;
                }
            }
        }
    }
}
