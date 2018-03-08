using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlantGeneticAlgorithm
{


    public class Rain : MonoBehaviour
    {
        bool started = false;
        Transform endPoint;
        public float speed = 1f;
        public void Init(Transform e)
        {
            endPoint = e;
            started = true;
        }
        private void Update()
        {
            if (started)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPoint.position, speed * Time.deltaTime);
                if (transform.position == endPoint.position)
                {
                    Destroy(gameObject);
                }
            }

        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.GetComponent<Plant>() != null)
            {

            }
        }
    }
}
