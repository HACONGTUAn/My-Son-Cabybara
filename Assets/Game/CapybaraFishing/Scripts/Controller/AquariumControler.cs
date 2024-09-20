using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class AquariumControler : MonoBehaviour
    {
        [SerializeField] private GameObject aquarium;
        [SerializeField] private GameObject fish;
        [SerializeField] private int maxFishCount = 10; 
        [SerializeField] private float initialDelay = 1f;
        [SerializeField] private float minDelay = 0.2f;
        private List<GameObject> fishList = new List<GameObject>();
        void OnEnable ()
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
                float delaySpawn = CalculateSpawnDelay(fishList.Count);              
                yield return new WaitForSeconds(delaySpawn);
                SpawmFish();
            }
        }
        private void SpawmFish()
        {
            int randomValue = Random.Range(0, 2) * 2 - 1;
            Vector3 posSpawn = new Vector3 (5 * randomValue, 0, 0);
            GameObject newFish = Instantiate(fish,posSpawn,Quaternion.identity);
            newFish.transform.SetParent(aquarium.transform, false);
            fishList.Add(newFish);
        }
        private float CalculateSpawnDelay(int currentFishCount)
        {          
            float ratio = (float)maxFishCount / (float)currentFishCount ;           
            return Mathf.Lerp(initialDelay, minDelay, ratio); 
            
        }
        private void OnDisable()
        {
            GameManager.Instance.fishingEvent -= SpawnFishHandler;
        }
    }
}
