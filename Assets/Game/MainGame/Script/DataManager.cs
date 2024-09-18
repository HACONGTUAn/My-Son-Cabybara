using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace CapybaraMain
{
    public class DataManager : TPRLSingleton<DataManager>
    {
        public List<MiniGame> _testData = new List<MiniGame>();
         List<MiniGame> _loadData;
        private void Start()
        {
           // WriteDataInJson();
            ReadFileJson();
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
        public void WriteDataInJson(List<MiniGame> listData)
        {
            string json = JsonConvert.SerializeObject(listData, Formatting.Indented);
            string path = Application.persistentDataPath + "/test.json";
            File.WriteAllText(path, json);
        }
        public List<MiniGame> ReadFileJson()
        {
            string path = Application.persistentDataPath + "/test.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);

                _loadData = JsonConvert.DeserializeObject<List<MiniGame>>(json);
  
                //foreach(var x in _loadData)
                //{
                //    Debug.Log(x.nameMinigame);
                //    foreach(var y in x.items)
                //    {
                //        Debug.Log("name Item : " + y.name + " item " + y.quantity);
                //    }
                //    Debug.Log(x.price);
                //}

                
            }
            return _loadData;
        }
    }
   
    public class Item
    {
        public string name;
        public int quantity;
        public Item(string _name, int _quantity)
        {
            name = _name;
            quantity = _quantity;
        }
    }
    public class MiniGame
    {
        public int id;
        public string nameMinigame;
        public List<Item> items;
        public int price;
    }
}