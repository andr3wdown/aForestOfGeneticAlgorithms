using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlantGeneticAlgorithm
{


    public class Spore : MonoBehaviour
    {
        Rigidbody2D rb;
        public LayerMask groundLayer;
        public PlantGenetics host;
        public float lifetime = 10f;
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        void Update()
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(gameObject);
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 1000, groundLayer);
            rb.AddForce(Vector3.up * 0.6f / (Vector2.Distance(transform.position, hit.point) + 0.1f));
            if (transform.position.y <= 0)
            {
                rb.AddForce(Vector3.up * 4f + new Vector3(Random.Range(-1f, 1f), 0, 0), ForceMode2D.Impulse);
                transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Seedling>() != null)
            {
                if (other.transform.parent != null)
                {
                    other.transform.root.GetComponent<Plant>().partner = host;
                }

            }
        }
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}
