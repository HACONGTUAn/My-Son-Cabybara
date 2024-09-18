using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraMain
{

public class Manager : TPRLSingleton<Manager>
{
        public int heart;
        public List<MiniGame> _data = new List<MiniGame>();

        private void LoadingDataInFile()
        {
            DataManager.Instance.WriteDataInJson(_data);
        }
        private void LoadingDataHeart()
        {

        }
        private void LoadingDataTicket()
        {

        }

        public void UpdateDataInFile(BaseID currentMiniGame)
        {
           int index = FindTypeOfObject(currentMiniGame);
            MiniGame _dataMiniGame = _data[index];

            _dataMiniGame.items = currentMiniGame.items;

            _data[index] = _dataMiniGame;
            Debug.Log("du lieu duoc ghi lai : " + _data[index].items[0].quantity);
            LoadingDataInFile();
        }

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
}

}