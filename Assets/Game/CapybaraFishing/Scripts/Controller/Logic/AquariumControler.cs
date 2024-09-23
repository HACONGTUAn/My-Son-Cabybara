using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class AquariumControler : MonoBehaviour
    {
        [SerializeField] private GameObject aquarium;
        [SerializeField] private GameObject[] fishes;
        [SerializeField] private int maxFishCount = 15; 
        [SerializeField] private float initialDelay = 3f;
        [SerializeField] private float minDelay = 0.2f;
        private List<GameObject> fishList = new List<GameObject>();
        private float[] spawnRates = { 0.4f, 0.3f, 0.15f, 0.1f, 0.05f }; 
      
        void Start ()
        {
            GameManager.Instance.fishingEvent += SpawnFishHandler;
        }
        public void SpawnFishHandler()
        {
            StartCoroutine(StartSpawn());
        }
        public void ClearAquarium()
        {
            foreach (GameObject fish in fishList)
            {
                Destroy(fish);
            }          
            fishList.Clear();
        }
        private IEnumerator StartSpawn()
        {
            while (GameManager.Instance.gameState == GameState.Fishing)
            {
                float delaySpawn = CalculateSpawnDelay(transform.childCount);               
                SpawmFish(); 
                yield return new WaitForSeconds(delaySpawn);
            }
        }
        private void SpawmFish()
        {
            int randomValue = Random.Range(0, 2) * 2 - 1;
            Vector3 posSpawn = new Vector3 (8 * randomValue, 0, 0);
            int fishIndex = GetRandomFishIndex();
            GameObject newFish = Instantiate(fishes[fishIndex],posSpawn,Quaternion.identity);
            newFish.transform.SetParent(aquarium.transform, false);
            fishList.Add(newFish);
        }
        private float CalculateSpawnDelay(int currentFishCount)
        {          
            float ratio = (float)currentFishCount/(float)maxFishCount ;           
            return Mathf.Lerp(minDelay, initialDelay, ratio); 
            
        }
        int GetRandomFishIndex()
        {
            float rand = Random.value;
            float cumulative = 0;

            for (int i = 0; i < spawnRates.Length; i++)
            {
                cumulative += spawnRates[i];
                if (rand < cumulative)
                {
                    return i;
                }
            }

            return spawnRates.Length - 1;
        }
        private void OnDestroy()
        {
            GameManager.Instance.fishingEvent -= SpawnFishHandler;
        }
    }
}
