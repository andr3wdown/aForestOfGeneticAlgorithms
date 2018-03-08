using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlantGeneticAlgorithm
{
    public class Root : MonoBehaviour
    {
        public float rootRating;
        public float rootMRating;
        public LayerMask rootLayer;
        Plant host;

        public void Init(Plant _h)
        {
            host = _h;
            Refresh();
        }
        public void Refresh()
        {
            rootRating = Mathf.Abs(transform.position.y) + 0.2f / 0.8f;
            rootMRating = Mathf.Abs(transform.root.position.x - transform.position.x) / 0.1f;
            Collider2D[] otherRoots = Physics2D.OverlapCircleAll(transform.position, 0.23f, rootLayer);
            int amount = 0;
            for (int i = 0; i < otherRoots.Length; i++)
            {
                if (otherRoots[i].transform.root.GetComponent<Plant>() != host)
                {
                    amount++;
                }
            }
            if (amount > 0)
            {
                rootMRating /= amount / 1.3f;
                rootRating /= amount / 2.3f;
            }
        }
    }
}
