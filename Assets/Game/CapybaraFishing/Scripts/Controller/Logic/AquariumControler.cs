using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class AquariumControler : MonoBehaviour
    {
        [SerializeField] private GameObject aquarium;
        [SerializeField] private GameObject[] fishes;
        [SerializeField] private GameObject[] trashes;
        [SerializeField] private int maxFishCount = 15; 
        [SerializeField] private float initialDelay = 3f;
        [SerializeField] private float minDelay = 0.2f;
        private List<GameObject> fishList = new List<GameObject>();
        private float[] spawnRates = { 0.4f, 0.3f, 0.1f, 0.1f, 0.1f };
        private bool isPause = false;
        void Start ()
        {
            GameManager.Instance.startEvent += InitializeAqua;
            GameManager.Instance.fishingEvent += SpawnFishHandler;
            GameManager.Instance.pause += SpawnPause;
            GameManager.Instance.unPause += SpawnUnPause;
        }
        private void InitializeAqua()
        {
            ClearAquarium();
            for (int i = 0; i < 5; i++)
            {
                SpawmFish();
            }
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
                yield return new WaitForSeconds(delaySpawn);
                if (isPause) continue;                            
                SpawmFish(); 
               

            }
        }
        private void SpawnPause()
        {
            isPause = true;
        }
        private void SpawnUnPause()
        {
            isPause = false;
        }
        private void SpawmFish()
        {
            int spawnValue = Random.Range(0, 7);
            if (spawnValue > 0)
            {
                int randomValue = Random.Range(0, 2) * 2 - 1;
                
                int fishIndex = GetRandomFishIndex();
                FishController fish = fishes[fishIndex].GetComponent<FishController>();
                float posX = Random.Range(fish.bottomArea.y, fish.topArea.y);
                float posY = 5 * randomValue;
                Vector3 posSpawn = new Vector3(posY, posX, 0);
                GameObject newFish = Instantiate(fishes[fishIndex], posSpawn, Quaternion.identity);
                newFish.transform.SetParent(aquarium.transform, false);
                fishList.Add(newFish);
            }
            else
            {
                int randomValue = Random.Range(0, 2) * 2 - 1;                
                int trashIndex = Random.Range(0, trashes.Length);              
                FishController fish = trashes[trashIndex].GetComponent<FishController>();
                float posX = Random.Range(fish.bottomArea.y, fish.topArea.y);
                float posY = 3 * randomValue;
                Vector3 posSpawn = new Vector3(posY, posX, 0);
                GameObject newFish = Instantiate(trashes[trashIndex], posSpawn, Quaternion.identity);
                newFish.transform.SetParent(aquarium.transform, false);
                fishList.Add(newFish);
            }
        }
        private float CalculateSpawnDelay(int currentFishCount)
        {          
            float ratio = (float)currentFishCount/(float)maxFishCount ;           
            return Mathf.Lerp(minDelay, initialDelay, ratio); 
            
        }
        private int GetRandomFishIndex()
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
            GameManager.Instance.startEvent -= InitializeAqua;
            GameManager.Instance.fishingEvent -= SpawnFishHandler;
            GameManager.Instance.pause -= SpawnPause;
            GameManager.Instance.unPause -= SpawnUnPause;
        }
    }
}
