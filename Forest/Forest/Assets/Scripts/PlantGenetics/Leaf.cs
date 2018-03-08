using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlantGeneticAlgorithm
{
    public class Leaf : MonoBehaviour
    {
        public static List<Leaf> allLeaves = new List<Leaf>();
        Plant host;
        public float leafRating = 1f;
        public float leafCRating = 1f;
        public float leafHealth = 1f;
        public LayerMask leafLayer;
        public void Init(Plant h)
        {
            host = h;
        }
        public void ReEvaluateLeaf()
        {
            leafCRating = 1f + (transform.position.y / 3f);
            leafRating = 1f + (Mathf.Abs(transform.root.position.x - transform.position.x) / 1f);
        }
        public void CheckDown()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector3.up, 10f, leafLayer);
            if (hit.transform != null && hit.transform.GetComponent<Leaf>() != null)
            {
                hit.transform.GetComponent<Leaf>().leafRating *= 0.75f;
            }
        }
        public void CheckUp()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector3.up, 10f, leafLayer);
            int incrementable = 0;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.root.GetComponent<Plant>() != host)
                {
                    incrementable++;
                }
            }
            if (incrementable > 0)
            {
                leafRating /= (incrementable * 1.5f);
            }
            Collider2D[] leaves = Physics2D.OverlapCircleAll(transform.position, 0.25f, leafLayer);


            if (leaves.Length > 0)
            {
                incrementable = 0;
                for (int i = 0; i < leaves.Length; i++)
                {
                    if (leaves[i].transform.root.GetComponent<Plant>() != host)
                    {
                        incrementable++;
                    }
                }
                if (incrementable > 0)
                {
                    leafCRating = 1f / (incrementable * 1.5f);
                }

            }

        }
        private void OnEnable()
        {
            allLeaves.Add(this);
        }
        private void OnDisable()
        {
            allLeaves.Remove(this);
        }
    }
}
