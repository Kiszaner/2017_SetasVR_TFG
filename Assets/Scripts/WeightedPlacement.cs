using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedPlacement : MonoBehaviour
{
    public GameObject[] mushroomsPrefabs;
    [Range(0f, 1f)]
    public float[] mushroomsChooseProbability;

    public int maxMushroomChoosen = 2;
    public Transform[] spawnPoints;
    public int maxSpawnChoosen = 3;

    public List<GameObject> InstantiatedTrees;
    public List<GameObject> InstantiatedMushrooms;

    private int[] mushroomChoosen;
    private Transform[] spawnPointsChoosen;

    private void OnEnable()
    {
        InstantiatedTrees = new List<GameObject>();
        InstantiatedMushrooms = new List<GameObject>();
    }

    public void TreeGroupPlacement()
    {
        //mushroomChoosen = new float[maxChoosen];
        mushroomChoosen = ChooseSet(mushroomsChooseProbability, maxMushroomChoosen);
        if (mushroomChoosen.Length < 1) return;
        spawnPointsChoosen = ChooseSet(spawnPoints, maxSpawnChoosen);

        foreach(Transform tf in spawnPointsChoosen)
        {
            int index = Choose(mushroomsChooseProbability);
            // Asuming that last probability in array is probability of no mushroom spawn
            if (index == mushroomsChooseProbability.Length - 1) return;
            SpawnMushroom(mushroomsPrefabs[index], tf);
        }
        int j = 0;
        foreach(int i in mushroomChoosen)
        {
            Debug.Log("j: " + j + ". i: " + i);
            j++;
        }
        //do
        //{
        //    int choosen = Choose(mushroomsChooseProbability);
        //    if(choosen)
        //} while (i < maxChoosen);
        //for (int i = 0; i < maxChoosen; i++)
        //{

        //}
    }

    private GameObject SpawnMushroom(GameObject mushroomPrefab, Transform spawnPoint)
    {
        return Instantiate(mushroomPrefab, spawnPoint.position, Quaternion.identity);
    }

    private int Choose(float[] probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

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
        return probs.Length - 1;
    }

    int[] ChooseSet(float[] probabilities, int numRequired)
    {
        int[] result = new int[numRequired];

        int numToChoose = numRequired;

        for (int numLeft = probabilities.Length; numLeft > 0; numLeft--)
        {

            float prob = (float)numToChoose / (float)numLeft;

            if (Random.value <= prob)
            {
                numToChoose--;
                result[numToChoose] = numLeft - 1; //probabilities[numLeft - 1];

                if (numToChoose == 0)
                {
                    break;
                }
            }
        }
        return result;
    }

    Transform[] ChooseSet(Transform[] spawnPoints, int numRequired)
    {
        Transform[] result = new Transform[numRequired];

        int numToChoose = numRequired;

        for (int numLeft = spawnPoints.Length; numLeft > 0; numLeft--)
        {

            float prob = (float)numToChoose / (float)numLeft;

            if (Random.value <= prob)
            {
                numToChoose--;
                result[numToChoose] = spawnPoints[numLeft - 1];

                if (numToChoose == 0)
                {
                    break;
                }
            }
        }
        return result;
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
