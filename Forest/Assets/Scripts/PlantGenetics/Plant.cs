using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Andr3wDown.Math;
using Andr3wDown.Algorithms;

namespace PlantGeneticAlgorithm
{
    public class Plant : MonoBehaviour
    {
        public PlantGenetics genes;

        bool started = false;
        public float stalkSize = 0.2f;
        public GameObject stalk;
        public GameObject branch;
        public GameObject[] leaves;
        public GameObject[] roots;
        public int plantLeafCount = 0;
        public int plantRootCount = 0;
        public int branchCount;

        public float vitality = 100f;
        public float minerals = 100f;
        public float photons = 100f;
        public float carbon = 100f;
        public float water = 100f;
        public float sugar = 100f;

        List<Leaf> leafList = new List<Leaf>();
        public List<Root> rootList = new List<Root>();

        public void InitializePlant(PlantGenetics _genes)
        {
            genes = _genes;
            transform.GetComponent<SpriteRenderer>().color = genes.colors[4].GetGradientColor(Random.Range(0f, 1f));
            Build();
        }
        void Build()
        {

            //float h = 0f;
            Quaternion dir = MathOperations.LookAt2D(transform.position + transform.up, transform.position, -90);
            for (int i = 0; i < genes.height; i++)
            {
                GameObject go = Instantiate(stalk, transform.position + (transform.up * (stalkSize * i)), dir);
                branchCount++;
                go.transform.parent = transform;
                go.GetComponent<SpriteRenderer>().color = genes.colors[0].GetGradientColor(Random.Range(0f, 1f));
            }
            List<GameObject> branches = new List<GameObject>();
            for (int i = 0; i < genes.branchCount; i++)
            {
                GameObject go = Instantiate(branch, transform.position + ((transform.up * (genes.height * stalkSize)) * genes.branchHeight[i]), Quaternion.Euler(dir.eulerAngles + new Vector3(0, 0, genes.branchRotations[i])));
                branchCount++;
                branches.Add(go);
                go.GetComponent<SpriteRenderer>().color = genes.colors[1].GetGradientColor(Random.Range(0f, 1f));
                go.transform.parent = transform;
                if (genes.branchLenght[i] > 1)
                {
                    for (int j = 0; j < genes.branchLenght[i] - 1; j++)
                    {
                        branchCount++;
                        GameObject gj = Instantiate(branch, go.transform.position + ((go.transform.up * stalkSize) + (go.transform.up * stalkSize * j)), go.transform.rotation);
                        gj.GetComponent<SpriteRenderer>().color = genes.colors[1].GetGradientColor(Random.Range(0f, 1f));
                        gj.transform.parent = transform;
                        branches.Add(gj);
                    }
                }
            }
            for (int i = 0; i < branches.Count; i++)
            {
                for (int j = 0; j < genes.leafCount; j++)
                {
                    GameObject go = Instantiate(Random.Range(0, 2) == 0 ? leaves[0] : leaves[1], branches[i].transform.position + (branches[i].transform.up * stalkSize), Quaternion.Euler(0, 0, branches[i].transform.eulerAngles.z + Random.Range(-genes.leafRotations[j].x, genes.leafRotations[j].y)));
                    go.GetComponent<SpriteRenderer>().color = genes.colors[2].GetGradientColor(Random.Range(0f, 1f));
                    go.transform.parent = transform;
                    plantLeafCount++;
                    go.GetComponent<Leaf>().Init(this);
                    leafList.Add(go.GetComponent<Leaf>());
                }

            }
            List<GameObject> currentRoots = new List<GameObject>();
            GameObject g = Instantiate(roots[Random.Range(0, 2)], transform.position, transform.rotation);
            g.GetComponent<SpriteRenderer>().color = genes.colors[3].GetGradientColor(Random.Range(0f, 1f));
            rootList.Add(g.GetComponent<Root>());
            g.transform.parent = transform;
            g.GetComponent<Root>().Init(this);
            currentRoots.Add(g);
            plantRootCount++;
            for (int i = 0; i < genes.rootCount; i++)
            {
                int index = genes.rootConnections[i];
                Transform current = currentRoots[index].transform;
                GameObject go = Instantiate(roots[Random.Range(0, 2)], current.position - (current.up * stalkSize), Quaternion.Euler(current.eulerAngles + new Vector3(0, 0, Random.Range(-genes.rootRotations[i].x, genes.rootRotations[i].y))));
                currentRoots.Add(go);
                go.GetComponent<SpriteRenderer>().color = genes.colors[3].GetGradientColor(Random.Range(0f, 1f));
                go.transform.parent = transform;
                go.GetComponent<Root>().Init(this);
                rootList.Add(go.GetComponent<Root>());
                plantRootCount++;
            }

            leafList = SortingAlgorithms.BubbleSort(leafList);
            for (int i = 0; i < leafList.Count; i++)
            {
                leafList[i].CheckDown();
                leafList[i].CheckUp();
            }

            started = true;
        }
        void Run()
        {
            sugar -= 0.003f * (plantLeafCount + branchCount + plantRootCount) * Time.deltaTime;
            if (sugar <= 0)
            {
                vitality -= 0.003f * (plantLeafCount + branchCount + plantRootCount);
            }
            if (carbon <= 0)
            {
                vitality -= 0.003f * (plantLeafCount + branchCount + plantRootCount);
            }
            if (water <= 0)
            {
                vitality -= 0.003f * (plantLeafCount + branchCount + plantRootCount);
            }

            if (vitality <= 0)
            {
                Destroy(gameObject);
            }
            for (int i = 0; i < rootList.Count; i++)
            {
                Intake("minerals", rootList[i].rootMRating * 0.22f);
                Intake("water", rootList[i].rootRating * 0.48f * RainMaker.GetMoisture);
            }
            for (int i = 0; i < leafList.Count; i++)
            {
                Intake("carbon", leafList[i].leafCRating * 0.18f);

                if (DayNightCycle.SunStrenght > 0.4f)
                {
                    Intake("photon", leafList[i].leafRating * 0.16f * DayNightCycle.SunStrenght);
                }


            }
            Photosynthesis(((plantLeafCount + plantRootCount) / 2f) * 0.031f, ((plantLeafCount + plantRootCount) / 2) * 0.026f, ((plantLeafCount + plantRootCount) / 2) * 0.12f);
            if (sugar > 75)
            {
                Morphosis(((branchCount + plantLeafCount) / 2) * 0.07f, ((branchCount + plantLeafCount) / 2) * 0.038f);
            }
            if (minerals > 75 && carbon > 50 || water > 75 && carbon > 25)
            {
                Inception(((branchCount + plantLeafCount) / 2) * 0.07f, ((branchCount + plantLeafCount) / 2) * 0.031f, ((branchCount + plantLeafCount) / 2) * 0.05f);
            }


        }
        float sporeReadiness = 0f;
        public GameObject spores;
        void Inception(float mineralConsumption, float waterConsumption, float carbonConsumption)
        {
            if (carbon <= 0)
            {
                return;
            }
            if (minerals <= 0)
            {
                return;
            }
            if (water <= 0)
            {
                return;
            }
            carbon -= carbonConsumption * Time.deltaTime;
            minerals -= mineralConsumption * Time.deltaTime;
            water -= waterConsumption * Time.deltaTime;
            sporeReadiness += 0.025f * ((carbonConsumption + waterConsumption + carbonConsumption) / 6f) * Time.deltaTime;
            if (sporeReadiness > 1f)
            {
                LaunchSpores();
                sporeReadiness = 0;
            }
        }
        void LaunchSpores(int sporeCount = 10)
        {
            for (int i = 0; i < sporeCount; i++)
            {
                GameObject go = Instantiate(spores, leafList[Random.Range(0, leafList.Count)].transform.position, Quaternion.identity);
                go.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-4f, 4f), 2f).normalized * 6f);
                go.GetComponent<SpriteRenderer>().color = genes.colors[5].GetGradientColor(Random.Range(0f, 1f));
                go.GetComponent<Spore>().host = this.genes;
            }
        }
        public GameObject seedling;
        GameObject growthling;
        public float growthlingSize = 0f;
        public PlantGenetics partner;


        void Morphosis(float mineralConsumption, float sugarConsumption)
        {
            if (minerals <= 0)
            {
                return;
            }
            if (sugar <= 0)
            {
                return;
            }
            if (growthling == null)
            {
                growthling = Instantiate(seedling, leafList[Random.Range(0, leafList.Count)].transform.position, Quaternion.identity);
                growthling.transform.parent = transform;
                growthling.GetComponent<SpriteRenderer>().color = genes.colors[4].GetGradientColor(Random.Range(0f, 1f));
                vitality -= 20;
                growthlingSize = 0;
            }
            else
            {

                growthlingSize += 0.027f * ((mineralConsumption + sugarConsumption) / 4f) * Time.deltaTime;
                if (growthlingSize >= 1f)
                {
                    DetachGrowthling();
                    growthlingSize = 1f;
                }
                if (growthlingSize < 0.99999f)
                {
                    minerals -= mineralConsumption * Time.deltaTime;
                    sugar -= sugarConsumption * Time.deltaTime;
                }
                if (growthling != null)
                {
                    growthling.transform.localScale = new Vector3(growthlingSize, growthlingSize, growthlingSize);
                }

            }



        }
        void DetachGrowthling()
        {
            if(growthlingSize < 1f)
            {
                growthling.transform.parent = null;
                growthling.GetComponent<Seedling>().Init();
                partner = null;
                growthling = null;
            }
            if (partner != null)
            {
                PlantGenetics newGenes = new PlantGenetics(genes, partner);
                growthling.transform.parent = null;
                growthling.GetComponent<Seedling>().Init(newGenes);
                partner = null;
                growthling = null;
            }
        }
        void Photosynthesis(float carbonConsumption, float waterConsumption, float photonConsumption)
        {
            if (carbon < 0)
            {
                return;
            }
            if (water < 0)
            {
                return;
            }
            if (photons < 0)
            {
                return;
            }
            float amount = (carbonConsumption + waterConsumption + photonConsumption) * Time.deltaTime * 0.3f;
            sugar += amount;
            carbon -= carbonConsumption * Time.deltaTime;
            water -= waterConsumption * Time.deltaTime;
            photons -= photonConsumption * Time.deltaTime;
        }
        public void Intake(string type, float rate)
        {
            if (type == "carbon")
            {
                carbon += rate * Time.deltaTime;
            }
            else if (type == "photon")
            {
                photons += rate * Time.deltaTime;
            }
            else if (type == "minerals")
            {
                minerals += rate * Time.deltaTime;
            }
            else if (type == "water")
            {
                water += rate * Time.deltaTime;
            }
        }
        Cooldown c = new Cooldown(5f);
        private void Update()
        {
            if(growthling != null)
            {
                if (growthlingSize < 0f)
                {
                    growthlingSize = 0f;
                    growthling.transform.localScale = new Vector3(0, 0, 0);
                    DetachGrowthling();
                }
            }
           
            if (started)
            {
                Run();
                if (c.CountDown())
                {
                    for (int i = 0; i < leafList.Count; i++)
                    {
                        leafList[i].ReEvaluateLeaf();
                        leafList[i].CheckDown();
                        leafList[i].CheckUp();

                    }
                    for (int i = 0; i < rootList.Count; i++)
                    {
                        rootList[i].Refresh();
                    }
                }
            }
        }

    }
    public class PlantGenetics
    {
        public int height;
        public int branchCount;
        public int leafCount;
        public int rootCount;
        public int[] rootConnections;
        public Vector2[] rootRotations;
        public int[] branchLenght;
        public float[] branchRotations;
        public float[] branchHeight;
        public Vector2[] leafRotations;
        public GradientObject[] colors;
        public PlantGenetics(int h, int b, int l, int r)
        {
            height = h;
            branchCount = b;
            leafCount = l;
            rootCount = r;
            rootConnections = new int[rootCount];
            rootRotations = new Vector2[rootCount];
            branchLenght = new int[branchCount];
            branchRotations = new float[branchCount];
            branchHeight = new float[branchCount];
            leafRotations = new Vector2[leafCount];
            colors = new GradientObject[6];

            for (int i = 0; i < rootCount; i++) { rootConnections[i] = Random.Range(0, i); rootRotations[i] = new Vector2(Random.Range(0f, 90f), Random.Range(0f, 90f)); }
            for (int i = 0; i < branchLenght.Length; i++) { branchLenght[i] = Random.Range(1, 4); }
            for (int i = 0; i < branchRotations.Length; i++) { branchRotations[i] = Random.Range(-67f, 67f); }
            for (int i = 0; i < branchHeight.Length; i++) { branchHeight[i] = Random.Range(0f, 1f); }
            for (int i = 0; i < leafRotations.Length; i++) { leafRotations[i] = new Vector2(Random.Range(0f, 90f), Random.Range(0f, 90f)); }
            for (int i = 0; i < colors.Length; i++) { colors[i] = GenerateRandomGradient(); }
        }
        public PlantGenetics(PlantGenetics female, PlantGenetics male, float mutationRate = 0.1f)
        {
            height = GetChance() ? Random.Range(2, 15) : (female.height + male.height) / 2;
            branchCount = GetChance() ? Random.Range(5, 30) : (female.branchCount + male.branchCount) / 2;
            leafCount = GetChance() ? Random.Range(1, 7) : (female.leafCount + male.leafCount) / 2;
            rootCount = GetChance() ? Random.Range(3, 15) : (female.rootCount + male.rootCount) / 2;
            rootConnections = MixAndMutateRootConnections(female.rootConnections, male.rootConnections, rootCount);
            rootRotations = MixAndMutateArrays(female.rootRotations, male.rootRotations, rootCount, 0f, 90f);
            branchLenght = MixAndMutateArrays(female.branchLenght, male.branchLenght, branchCount, 1, 6);
            branchRotations = MixAndMutateArrays(female.branchRotations, male.branchRotations, branchCount, -67f, 67f);
            branchHeight = MixAndMutateArrays(female.branchHeight, male.branchHeight, branchCount, 0f, 1f);
            leafRotations = MixAndMutateArrays(female.leafRotations, male.leafRotations, leafCount, 0f, 90f);
            colors = MixAndMutateArrays(female.colors, male.colors, 6);
        }
        public int[] MixAndMutateRootConnections(int[] arr1, int[] arr2, int lenght)
        {
            int[] newArr = new int[lenght];
            int l1 = arr1.Length;
            int l2 = arr2.Length;
            for (int i = 0; i < lenght; i++)
            {
                if (i < l1 && i < l2)
                {
                    newArr[i] = GetChance() ? Random.Range(0, i) : GetChance() ? arr1[i] : arr2[i];
                    if (i == 0 && newArr[i] > 0)
                    {
                        newArr[i] = 0;
                    }
                }
                else
                {
                    if (i < l1)
                    {
                        newArr[i] = GetChance() ? Random.Range(0, i) : arr1[i];
                    }
                    else if (i < l2)
                    {
                        newArr[i] = GetChance() ? Random.Range(0, i) : arr2[i];
                    }
                    else
                    {
                        newArr[i] = Random.Range(0, i);
                    }

                }
            }
            return newArr;
        }
        public Vector2[] MixAndMutateArrays(Vector2[] arr1, Vector2[] arr2, int lenght, float rangeMin, float rangeMax)
        {
            Vector2[] newArr = new Vector2[lenght];
            int l1 = arr1.Length;
            int l2 = arr2.Length;
            for (int i = 0; i < lenght; i++)
            {
                if (i < l1 && i < l2)
                {
                    newArr[i] = GetChance() ? new Vector2(Random.Range(rangeMin, rangeMax), Random.Range(rangeMin, rangeMax)) : (arr1[i] + arr2[i]) / 2f;
                }
                else
                {
                    if (i < l1)
                    {
                        newArr[i] = GetChance() ? new Vector2(Random.Range(rangeMin, rangeMax), Random.Range(rangeMin, rangeMax)) : arr1[i];
                    }
                    else if (i < l2)
                    {
                        newArr[i] = GetChance() ? new Vector2(Random.Range(rangeMin, rangeMax), Random.Range(rangeMin, rangeMax)) : arr2[i];
                    }
                    else
                    {
                        newArr[i] = new Vector2(Random.Range(rangeMin, rangeMax), Random.Range(rangeMin, rangeMax));
                    }

                }
            }
            return newArr;
        }
        public float[] MixAndMutateArrays(float[] arr1, float[] arr2, int lenght, float rangeMin, float rangeMax)
        {
            float[] newArr = new float[lenght];
            int l1 = arr1.Length;
            int l2 = arr2.Length;
            for (int i = 0; i < lenght; i++)
            {
                if (i < l1 && i < l2)
                {
                    newArr[i] = GetChance() ? Random.Range(rangeMin, rangeMax) : (arr1[i] + arr2[i]) / 2f;
                }
                else
                {
                    if (i < l1)
                    {
                        newArr[i] = GetChance() ? Random.Range(rangeMin, rangeMax) : arr1[i];
                    }
                    else if (i < l2)
                    {
                        newArr[i] = GetChance() ? Random.Range(rangeMin, rangeMax) : arr2[i];
                    }
                    else
                    {
                        newArr[i] = Random.Range(rangeMin, rangeMax);
                    }

                }
            }
            return newArr;
        }
        public GradientObject[] MixAndMutateArrays(GradientObject[] arr1, GradientObject[] arr2, int lenght)
        {
            GradientObject[] newArr = new GradientObject[lenght];
            int l1 = arr1.Length;
            int l2 = arr2.Length;
            for (int i = 0; i < lenght; i++)
            {
                if (i < l1 && i < l2)
                {
                    newArr[i] = GetChance() ? GenerateRandomGradient() : MixGradients(arr1[i], arr2[i]);
                }
                else
                {
                    if (i < l1)
                    {
                        newArr[i] = GetChance() ? GenerateRandomGradient() : arr1[i];
                    }
                    else if (i < l2)
                    {
                        newArr[i] = GetChance() ? GenerateRandomGradient() : arr2[i];
                    }
                    else
                    {
                        newArr[i] = GenerateRandomGradient();
                    }

                }
            }
            return newArr;
        }
        public int[] MixAndMutateArrays(int[] arr1, int[] arr2, int lenght, int rangeMin, int rangeMax)
        {
            int[] newArr = new int[lenght];
            int l1 = arr1.Length;
            int l2 = arr2.Length;
            for (int i = 0; i < lenght; i++)
            {
                if (i < l1 && i < l2)
                {
                    newArr[i] = GetChance() ? Random.Range(rangeMin, rangeMax) : Mathf.RoundToInt((arr1[i] + arr2[i]) / 2f);
                    if (newArr[i] <= 0)
                    {
                        newArr[i] = 1;
                    }
                }
                else
                {
                    if (i < l1)
                    {
                        newArr[i] = GetChance() ? Random.Range(rangeMin, rangeMax) : arr1[i];
                    }
                    else if (i < l2)
                    {
                        newArr[i] = GetChance() ? Random.Range(rangeMin, rangeMax) : arr2[i];
                    }
                    else
                    {
                        newArr[i] = Random.Range(rangeMin, rangeMax);
                    }

                }
            }
            return newArr;
        }
        bool GetChance(float chance = 0.1f)
        {
            float r = Random.Range(0f, 1f);
            if (r < chance)
            {
                return true;
            }
            return false;
        }
        public Color GenerateRandomColor()
        {
            Color c = Color.white;
            c.r = Random.Range(0f, 1f);
            c.g = Random.Range(0f, 1f);
            c.b = Random.Range(0f, 1f);
            return c;
        }
        public GradientObject GenerateRandomGradient()
        {
            return new GradientObject(GenerateRandomColor(), GenerateRandomColor());
        }
        public GradientObject MixGradients(GradientObject c1, GradientObject c2)
        {
            return new GradientObject(Color.Lerp(c1.color1, c2.color1, 0.5f), Color.Lerp(c1.color2, c2.color2, 0.5f));
        }
    }
}


