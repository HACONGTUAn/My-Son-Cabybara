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
        private int count = 15;
        public float carpetHeight;
        public Queue<GameObject> carpetList = new Queue<GameObject>();

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Start()
        {

            carpetHeight = carpetPrefab.transform.GetChild(1).gameObject.GetComponent<Renderer>().bounds.size.y - 0.6f;
        }

        // Update is called once per frame
        void Update()
        {

        }


        public GameObject GetObject(int index, Transform trans)
        {

            if (carpetList.Count >= count && !carpetList.Peek().GetComponent<CapybaraCarpet>().isMoving)
            {
                GameObject carpet = carpetList.Dequeue();

                carpet.transform.GetChild(0).transform.localPosition = new Vector3(2.53f, -0.07f, 0);
                carpetList.Enqueue(carpet);
                if (trans.gameObject.name == "leftPos")
                {
                    carpet.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
                    carpet.GetComponent<CapybaraCarpet>().isRight = false;
                }
                else
                {
                    carpet.GetComponent<CapybaraCarpet>().isRight = true;
                    carpet.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                }

                carpet.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = this.listCarpet[index];
                carpet.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = GameManager.Instance.initSortingLayer++;
                carpet.SetActive(true);

                return carpet;
            }
            GameObject obj = Instantiate(carpetPrefab, this.transform);
            if (trans.gameObject.name == "leftPos")
            {
                obj.GetComponent<CapybaraCarpet>().isRight = false;
                obj.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
            else
            {
               obj.GetComponent<CapybaraCarpet>().isRight = true;
               obj.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            obj.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = this.listCarpet[index];
            obj.transform.GetChild(0).transform.localPosition = new Vector3(2.53f, -0.07f, 0);
            obj.transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = GameManager.Instance.initSortingLayer++;
            carpetList.Enqueue(obj);
            return obj;
        }
    }
}
