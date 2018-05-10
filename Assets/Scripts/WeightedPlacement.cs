using System.Collections;
using System.Collections.Generic;
using UBUSetasVR;
using UnityEngine;
using UnityEngine.Assertions;

public class WeightedPlacement : MonoBehaviour
{
    public Mushroom[] mushrooms;
    public GameObject[] trees;
    public Transform TreeContainer;
    public Transform MushroomContainer;

    public int numberOfTrees = 12;
    public int numberOfTreesPerGroup = 5;
    public float treePlacementRadius = 20;

    public int maxNumMushroomsSpawned;
    public int maxMushroomChosen = 2;
    public float spawnMinRadius = 1f;
    public float spawnMaxRadius = 2f;
    public int spawnPointsAroundTrees = 7;
    public int maxSpawnPointsChosen = 3;

    public List<GameObject> InstantiatedTrees;
    public List<GameObject> InstantiatedMushrooms;

    private Mushroom[] mushroomsChosen;
    private Vector3[] spawnPointsChosen;
    private Vector3[] spawnPoints;

    private void OnEnable()
    {
        InstantiatedTrees = new List<GameObject>();
        InstantiatedMushrooms = new List<GameObject>();
    }

    public void TreeGroupPlacement()
    {
        // Make Tree group
        Transform treeGroup;
        int treesLeft = numberOfTrees;
        int treesWithoutPlace = numberOfTrees % numberOfTreesPerGroup;
        List<GameObject> treesFromGroup;
        int k = 0;
        for (k = 0; treesLeft > 0; k++)
        {
            treeGroup = new GameObject("TreeGroup" + k).transform;
            treeGroup.transform.SetParent(TreeContainer);
            treesFromGroup = new List<GameObject>();
            // Spawn Trees randomly
            if (treesLeft >= numberOfTreesPerGroup)
            {
                for (int j = 0; j < numberOfTreesPerGroup; j++)
                {
                    treesFromGroup.Add(SpawnTree(treeGroup));
                }
            }
            treesLeft -= numberOfTreesPerGroup;
            if (treesLeft < 0)
            {
                for (int j = 0; j < treesWithoutPlace; j++)
                {
                    treesFromGroup.Add(SpawnTree(treeGroup));
                }
            }

            SpawnMushroomsFromGroup(treesFromGroup);
        }
    }

    private void SpawnMushroomsFromGroup(List<GameObject> treesFromGroup)
    {
        if (IsMushroomLimitReached()) return;
        float[] mushroomsProbabilities;
        // Choose posible mushrooms to spawn in this group of trees (random at the moment)
        mushroomsProbabilities = mushrooms.GetProbabilities();
        mushroomsChosen = ChooseSet(mushroomsProbabilities, maxMushroomChosen);
        int i = 0;
        foreach (Mushroom mush in mushroomsChosen)
        {
            Debug.Log("Chosen " + i + ": " + mush.prefab.name);
            i++;
        }
        if (mushroomsChosen.Length < 1) return;

        // Run through trees from the group
        foreach (GameObject tree in treesFromGroup)
        {
            if (IsMushroomLimitReached()) return;
            //Choose posible points around to spawn
            spawnPoints = GetPointsAround(tree.transform.position, spawnMaxRadius, spawnMinRadius, spawnPointsAroundTrees);

            spawnPointsChosen = ChooseSet(spawnPoints, maxSpawnPointsChosen);
            i = 0;
            foreach (Vector3 vec3 in spawnPointsChosen)
            {
                //Debug.Log("Chosen " + i + ": " + vec3);
                i++;
            }
            foreach (Vector3 vec3 in spawnPointsChosen)
            {
                // Choose a mushroom index (or not) from chosen mushrooms to spawn in the point
                float[] mushroomsChosenProbabilities = mushroomsChosen.GetProbabilities();
                int index = Choose(mushroomsChosenProbabilities);
                if (index != -1)
                {
                    // Spawn the mushroom if one is chosen with random rotation
                    SpawnMushroom(mushroomsChosen[index].prefab, vec3);
                }
            }
        }
    }

    private bool IsMushroomLimitReached()
    {
        // To limit mushrooms spawned
        return maxNumMushroomsSpawned >= 0 && InstantiatedMushrooms.Count >= maxNumMushroomsSpawned;
    }

    private GameObject SpawnTree(Transform treeGroup)
    {
        Vector3 pos;
        Quaternion rot;
        GameObject tmp;
        GameObject go = trees[Random.Range(0, trees.Length)];
        pos = Random.insideUnitCircle * treePlacementRadius;
        rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        tmp = Instantiate(go, new Vector3(pos.x, 0f, pos.y), rot);
        tmp.transform.SetParent(treeGroup);
        InstantiatedTrees.Add(tmp);
        return tmp;
    }

    private Vector3[] GetPointsAround(Vector3 position, float maxRadius, float minRadius, int maxPoints)
    {
        Vector3[] points = new Vector3[maxPoints];
        for (int i = 0; i < maxPoints; i++)
        {
            points[i]= AuxiliarFunctions.PickRandomPosAroundPoint(position, maxRadius, minRadius);
        }
        return points;
    }

    private GameObject SpawnMushroom(GameObject mushroomPrefab, Vector3 spawnPoint)
    {
        if (IsMushroomLimitReached()) return null;
        GameObject mushroom;
        Quaternion rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        mushroom = Instantiate(mushroomPrefab, spawnPoint, rotation);
        mushroom.transform.SetParent(MushroomContainer);
        InstantiatedMushrooms.Add(mushroom);
        return mushroom;
    }

    public void RepeatPlacement()
    {
        ClearList(InstantiatedTrees);
        ClearList(InstantiatedMushrooms);
        TreeGroupPlacement();
    }

    public void ClearList(List<GameObject> list)
    {
        if (list != null)
        {
            foreach (GameObject go in list)
            {
                RemoveMushroomFromList(go);
            }
            list.Clear();
        }
    }

    public void RemoveMushroomFromList(GameObject go)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            DestroyImmediate(go);
        }
        else
        {
            Destroy(go);
        }
#elif UNITY_ANDROID
                Destroy(go);
#endif
    }

    // Choosing Items with Different Probabilities
    private int Choose(float[] probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            // Probability of being chosen: prob
            total += elem;
            // Probability of not being chosen: 1 - prob
            total += 1 - elem;
        }

        //Debug.Log("Total: " + total);

        float randomPoint = Random.value * total;

        //Debug.Log("Point: " + randomPoint);
        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return -1;
    }

    // Sin repeticiones int
    private Mushroom[] ChooseSet(float[] probabilities, int numRequired)
    {
        Mushroom[] result = new Mushroom[numRequired];

        int numToChoose = numRequired;

        for (int numLeft = probabilities.Length; numToChoose > 0 && numLeft > 0; numLeft--)
        {
            float prob = (float)numToChoose / (float)numLeft;

            if (Random.value <= prob)
            {
                numToChoose--;
                result[numToChoose] = mushrooms[numLeft - 1]; //probabilities[numLeft - 1];
            }
        }
        return result;
    }

    // Sin repeticiones Transform
    private Vector3[] ChooseSet(Vector3[] spawnPoints, int numRequired)
    {
        Vector3[] result = new Vector3[numRequired];

        int numToChoose = numRequired;

        for (int numLeft = spawnPoints.Length; numToChoose > 0 && numLeft > 0; numLeft--)
        {

            float prob = (float)numToChoose / (float)numLeft;

            if (Random.value <= prob)
            {
                numToChoose--;
                result[numToChoose] = spawnPoints[numLeft - 1];
            }
        }
        return result;
    }

    [System.Serializable]
    public struct Mushroom
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float probability;

    }

        //public int GetRandomWeightedIndex(int[] weights)
        //{
        //    // Get the total sum of all the weights.
        //    int weightSum = 0f;
        //    for (int i = 0; i < weights; ++i)
        //    {
        //        weightSum += weights[i];
        //    }

        //    // Step through all the possibilities, one by one, checking to see if each one is selected.
        //    int index = 0;
        //    int lastIndex = elementCount - 1;
        //    while (index < lastIndex)
        //    {
        //        // Do a probability check with a likelihood of weights[index] / weightSum.
        //        if (Random.Range(0, weightSum) < weights[index])
        //        {
        //            return index;
        //        }

        //        // Remove the last item from the sum of total untested weights and try again.
        //        weightSum -= weights[index++];
        //    }

        //    // No other item was selected, so return very last index.
        //    return index;
        //}
    }

public static class MushroomExtension
{
    public static float[] GetProbabilities(this WeightedPlacement.Mushroom[] mushrooms)
    {
        float[] result = new float[mushrooms.Length];
        for (int i = 0; i < mushrooms.Length; i++)
        {
            result[i] = mushrooms[i].probability;
        }
        return result;
    }

    public static GameObject[] GetPrefabs(this WeightedPlacement.Mushroom[] mushrooms)
    {
        GameObject[] result = new GameObject[mushrooms.Length];
        for (int i = 0; i < mushrooms.Length; i++)
        {
            result[i] = mushrooms[i].prefab;
        }
        return result;
    }
}