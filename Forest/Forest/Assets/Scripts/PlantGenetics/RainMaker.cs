using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantGeneticAlgorithm
{


    public class RainMaker : MonoBehaviour
    {
        GameObject currentCloud;
        public GameObject cloud;
        public float rainChance = 0.2f;
        public Cooldown rainC;
        public Transform
            spawn1,
            spawn2;

        public float moisture = 1f;
        public float moistureDecreaseRate = 0.001f;
        static RainMaker instance;
        private void Start()
        {
            instance = this;
        }
        public void Update()
        {
            moisture -= moistureDecreaseRate * Time.deltaTime;
            if (moisture <= 0)
            {
                moisture = 0;
            }
            if (currentCloud == null && rainC.CountDown())
            {
                CheckForRain();
            }

            if (currentCloud != null)
            {
                moisture = 1f;
            }

        }
        void CheckForRain()
        {
            float chance = Random.Range(0f, 1f);
            if (chance <= rainChance)
            {
                int c = Random.Range(0, 2);
                GameObject go = Instantiate(cloud, c == 0 ? spawn1.position : spawn2.position, Quaternion.identity);
                currentCloud = go;
                go.GetComponent<Rain>().Init(c == 1 ? spawn1 : spawn2);
                moisture = 1f;
            }
        }
        public static float GetMoisture
        {
            get
            {
                return instance.moisture;
            }
        }
    }
}