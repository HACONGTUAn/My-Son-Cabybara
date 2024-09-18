using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraMain
{

    public class LoadingResources : TPRLSingleton<LoadingResources>
    {
        // Start is called before the first frame update
        [SerializeField] private string pathMiniGameInResources = "MiniGame";
        public Dictionary<int,GameObject> keyValuePairs = new Dictionary<int,GameObject>();
        public List<BaseID> id;
        void Start()
         {
            LoadingAllGameObjectInResources();
         
         }

         private void LoadingAllGameObjectInResources()
          {
            GameObject[] allObject = Resources.LoadAll<GameObject>(pathMiniGameInResources);
            if(allObject.Length <= 0 || allObject == null)
            {
                Debug.Log("Loading false");
                return;
            }
            for(int i = 0; i < allObject.Length; i++)
            {
                keyValuePairs.Add(i, allObject[i]);
                if(allObject[i].name == id[i].name)
                {
                    id[i].id = i;
                }
            }
          }
    }

}