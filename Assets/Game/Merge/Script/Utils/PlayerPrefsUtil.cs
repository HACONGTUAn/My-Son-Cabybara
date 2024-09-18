using System;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Merge
{
    public class PlayerPrefsUtil
    {
        public static int CLBannerReloadTime
        {
            get { return PlayerPrefsBase.Instance().getInt("banner_collapse_time", 60); }
            set { PlayerPrefsBase.Instance().setInt("banner_collapse_time", value); }
        }
        public static string CF_CollapseBanner
        {
            get { return PlayerPrefsBase.Instance().getString("cf_cl_banner", "3,45"); }
            set { PlayerPrefsBase.Instance().setString("cf_cl_banner", value); }
        }
        public static string CF_NewFruitMerge
        {
            get { return PlayerPrefsBase.Instance().getString("cf_new_fruit_merge", "1,6"); }
            set { PlayerPrefsBase.Instance().setString("cf_new_fruit_merge", value); }
        }
    }
}