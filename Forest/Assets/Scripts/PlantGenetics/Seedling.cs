using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlantGeneticAlgorithm
{
    public class Seedling : MonoBehaviour
    {
        Rigidbody2D rb;
        public Cooldown c;
        public float growTime = 10f;
        bool active = true;
        bool dir;
        public GameObject plant;
        public PlantGenetics genes;
        float startTimer = 0.2f;
        bool started = false;
        public CircleCollider2D disableable;
        public float vitality
        {
            get
            {
                return transform.localScale.x * 100f;
            }
        }
        public void Init(PlantGenetics _genes)
        {
            dir = Random.Range(0, 2) == 0;
            genes = _genes;
            rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            started = true;
        }
        public void Init()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints2D.None;
        }
        private void Update()
        {
            if (started)
            {
                if (startTimer < 0)
                {
                    rb.gravityScale = 0;
                    rb.drag = 5;
                    if (active)
                    {
                        if (c.CountDown(true, Random.Range(0.1f, 10f)))
                        {
                            dir = !dir;
                        }
                        rb.AddForce(new Vector2(dir ? -2 : 2, 0).normalized * Random.Range(2f, 3f) - (Vector2.up * 2f));
                    }
                    else
                    {
                        rb.velocity = Vector2.zero;
                        rb.angularVelocity = 0;
                    }
                }
                else
                {
                    startTimer -= Time.deltaTime;
                }
            }



        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (started && other.gameObject.tag == "Ground")
            {
                disableable.enabled = false;
                active = false;
                StartCoroutine(GrowthTimer());
            }

        }
        IEnumerator GrowthTimer()
        {
            yield return new WaitForSeconds(growTime);
            SpawnPlant();
        }
        void SpawnPlant()
        {

            GameObject go = Instantiate(plant, transform.position, transform.rotation);
            go.GetComponent<Plant>().InitializePlant(genes);
            Destroy(gameObject);
        }
    }
}
