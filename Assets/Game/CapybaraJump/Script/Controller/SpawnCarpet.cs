using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapybaraJump
{
    public class SpawnCarpet : MonoBehaviour
    {

        public static SpawnCarpet Instance;

        // Start is called before the first frame update
        [SerializeField] List<Transform> spawnPosList;
        public List<Transform> oldPosList;
        public Queue<CapybaraCarpet> queueCarpet;
        public bool isMoving = false;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void SpawnNewCarpet(float step)
        {
            int index = Random.Range(0, 2);
            if (spawnPosList[index] == null)
            {
                Debug.Log("spawnPos is Null");
                return;
            }
            GameObject newCarpet = InstantiateGameObject.Instance.GetObject(0, spawnPosList[index]);
            Vector3 startPos = spawnPosList[index].position;
            Vector3 targetPos = new Vector3(0f, startPos.y, startPos.z);
            newCarpet.GetComponent<CapybaraCarpet>().MoveToCenter(startPos, targetPos, step);

        }

        public void UpdatePos()
        {
            spawnPosList[0].position += Vector3.up * InstantiateGameObject.Instance.carpetHeight;
            spawnPosList[1].position += Vector3.up * InstantiateGameObject.Instance.carpetHeight;
        }

    }

}
