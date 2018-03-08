using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantGeneticAlgorithm
{
    public class SeedlingSpawner : MonoBehaviour
    {
        public GameObject seedling;
        public GameObject spawnPoint;
        public int amount = 14;

        void Start()
        {

            for (int i = -amount; i <= amount; i++)
            {
                GameObject go = Instantiate(seedling, spawnPoint.transform.position + new Vector3(i * ((float)10 / amount), 0, 0), spawnPoint.transform.rotation);
                go.GetComponent<Seedling>().Init(new PlantGenetics(Random.Range(2, 4), Random.Range(5, 20), Random.Range(1, 5), Random.Range(3, 10)));
            }
        }


    }
}
