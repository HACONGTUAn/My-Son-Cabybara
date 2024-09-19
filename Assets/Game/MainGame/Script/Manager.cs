using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CapybaraMain
{

public class Manager : TPRLSingleton<Manager>
{
        public TMP_Text hearText;
        public TMP_Text teckitText;
        public List<MiniGame> _data = new List<MiniGame>();

        private int heart = 0;
        private int ticket = 0;

        private void Start()
        {
            // heart
            if (!PlayerPrefs.HasKey("heart"))
            {
                PlayerPrefs.SetInt("heart", heart);
            }
            else
            {
                //hearText.text = PlayerPrefs.GetInt("heart").ToString();
            }
            // teckit
            if (!PlayerPrefs.HasKey("teckit"))
            {
                PlayerPrefs.SetInt("teckit", ticket);
            }
            else
            {
               // teckitText.text = PlayerPrefs.GetInt("teckit").ToString();
            }

        }

        // write data 
        //==========================================================================
        private void WriteDataInFile()
        {
            DataManager.Instance.WriteDataInJson(_data);
        }
        private void WriteDataHeart()
        {
          
            PlayerPrefs.SetInt("heart",heart);
            PlayerPrefs.Save();
        }
        private void WriteDataTicket()
        {
            PlayerPrefs.SetInt("teckit", ticket);
            PlayerPrefs.Save();
        }
        //==========================================================================

        // call method
        //data in item 
        //==========================================================================
        //Set Data
        public void UpdateDataInFile(BaseID currentMiniGame)
        {
            int index = FindTypeOfObject(currentMiniGame);
            if (index < 0) {Debug.Log("No find type Object"); return; }
            MiniGame _dataMiniGame = _data[index];

            _dataMiniGame.items = currentMiniGame.items;

            _data[index] = _dataMiniGame;
            WriteDataInFile();
        }
        // Get Data
        public List<Item> ReadDataInFile(BaseID currentMiniGame)
        {
            int index = FindTypeOfObject(currentMiniGame);
            return _data[index].items;
        }

        public int FindTypeOfObject(BaseID currentMiniGame)
        {
            _data = DataManager.Instance.ReadFileJson();
            for(int i = 0; i < _data.Count; i++)
            {
                if(_data[i].id == currentMiniGame.id)
                {
                    return i;
                }
            }
            return -1;
        }
        //===================================================================================

        //call data heart
        //===================================================================================
        public void SetHeart(int _heart)
        {
            heart = _heart;
            WriteDataHeart();
        }

        public int GetHeart()
        {
            return heart;
        }

        //===================================================================================

        //call data teckit
        //===================================================================================
        public void SetTicket(int _teckit)
        {
            ticket = _teckit;
            WriteDataTicket();
        }

        public int GetTicket()
        {
            return ticket;
        }
        //===================================================================================

    }

}