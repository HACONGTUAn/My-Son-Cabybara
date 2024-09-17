using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Twisted
{
    public class HeartManager : Singleton<HeartManager>
    {
        public static readonly int MAX_HEART = 5;
        private string lastTime
        {
            get { return PlayerPrefs.GetString("last_heart_time", ""); }
            set { PlayerPrefs.SetString("last_heart_time", value); }
        }
        private void Start()
        {
            DataManager.OnAddHeart += OnAddHeart;
            CheckOffline();
        }

        private void Update()
        {
            if (DataManager.Heart >= MAX_HEART) return;
            DateTime now = DateTime.Now;
            DateTime recoverTime = DateTime.Parse(lastTime);
            recoverTime.AddMinutes(10);
            TimeSpan defl = TimeSpan.FromMinutes(10);
            TimeSpan r = defl - (now - recoverTime);
            if (r.TotalSeconds <= 0)
            {
                DataManager.IncreaseHeart(1);
                lastTime = now.ToString();
            }
        }
        public void OnAddHeart(int newValue)
        {
           
            if (newValue < MAX_HEART && lastTime.Length == 0)
            {
                lastTime =DateTime.Now.ToString();
            }
            if (newValue == MAX_HEART)
            {
                lastTime = "";
            }
        }
        public string GetTimeRemaningText()
        {
            if (DataManager.Heart < MAX_HEART)
            {
                DateTime now =DateTime.Now;
                DateTime recoverTime = DateTime.Parse(lastTime);
                recoverTime.AddMinutes(30);
                TimeSpan defl = TimeSpan.FromMinutes(30);
                TimeSpan r = defl - (now - recoverTime);
                return r.ToString(@"mm\:ss");
            }
            return "Full";
        }
        private void CheckOffline()
        {
            if (lastTime.Length == 0) return;
            DateTime now = DateTime.Now;
            DateTime recoverTime = DateTime.Parse(lastTime);
            TimeSpan r = now - recoverTime;
            int lives = (int)(r.TotalMinutes / 30);
            int maxLives = MAX_HEART - DataManager.Heart;
            if (lives > maxLives)
            {
                lives = maxLives;
            }
            if (lives > 0)
            {
                DataManager.IncreaseHeart(lives);
            }
        }
    }
}