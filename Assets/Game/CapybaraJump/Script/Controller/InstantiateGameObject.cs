using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace CapybaraJump
{
    public class InstantiateGameObject : MonoBehaviour
    {

        public static InstantiateGameObject Instance;

        [SerializeField] private List<Sprite> listCarpet = new();
        [SerializeField] private GameObject carpetPrefab;
        private int count = 8;
        public float carpetHeight;
        public Queue<GameObject> carpetList = new Queue<GameObject>();

        void Awake(){
            if(Instance == null){
                Instance = this;
            }
        }

        void Start()
        {
            
            carpetHeight = carpetPrefab.transform.GetChild(1).gameObject.GetComponent<Renderer>().bounds.size.y-0.1f;
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        public GameObject GetObject(int index, Transform trans){
            
            if(carpetList.Count == count){
                GameObject carpet = carpetList.Dequeue();
                if(trans.gameObject.name == "leftPos"){
                   carpet.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                }
                else {
                    carpet.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                }
                carpet.transform.GetChild(0).transform.localPosition = new Vector3(2.18f, -0.4197f, 0);
                carpet.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = this.listCarpet[index];
                //obj.SetActive(true);
                carpetList.Enqueue(carpet);
                return carpet;
            }
            GameObject obj = Instantiate(carpetPrefab, this.transform);
            if(trans.gameObject.name == "leftPos"){
                obj.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            obj.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = this.listCarpet[index];
            carpetList.Enqueue(obj);
            return obj;
        }
    }
}
