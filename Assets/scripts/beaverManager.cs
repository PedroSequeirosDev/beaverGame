using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class beaverManager : MonoBehaviour
{
    public GameObject beaverPrefab; // Assign in Inspector
    public Transform spawnPointA;   // Assign in Inspector
    public Transform spawnPointB;   // Assign in Inspector

    public int maxPopulation = 0;
    public int beaversPerHouse = 5;
    public List<beaverAI> allBeavers = new List<beaverAI>();

    void Update()
    {
        int houseCount = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Count(obj => obj.CompareTag("House"));

        maxPopulation = houseCount * beaversPerHouse;

        int currentBeavers = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Count(obj => obj.CompareTag("Beaver"));

        while (currentBeavers < maxPopulation)
        {
            GameObject beaver = Instantiate(beaverPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            var ai = beaver.GetComponent<beaverAI>();
            if (ai != null)
            {
                ai.profession = BeaverProfession.Idle;
                // Find a house with less than 5 idle beavers
                var houses = GameObject.FindGameObjectsWithTag("House");
                GameObject chosenHouse = null;
                foreach (var house in houses)
                {
                    int idleCount = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                        .Count(obj =>
                        {
                            var otherAI = obj.GetComponent<beaverAI>();
                            return otherAI != null &&
                                otherAI.profession == BeaverProfession.Idle &&
                                otherAI.targetObject == house;
                        });
                    if (idleCount < beaversPerHouse)
                    {
                        chosenHouse = house;
                        break;
                    }
                }
                // If all houses are full, just pick a random one
                if (chosenHouse == null && houses.Length > 0)
                    chosenHouse = houses[Random.Range(0, houses.Length)];
                ai.targetObject = chosenHouse;
                allBeavers.Add(ai);
            }
            currentBeavers++;
        }

        Vector3 GetRandomSpawnPosition()
        {
            if (spawnPointA == null || spawnPointB == null)
            {
                Debug.LogWarning("Spawn points not assigned!");
                return Vector3.zero;
            }
            return Random.value < 0.5f ? spawnPointA.position : spawnPointB.position;
        }
    }
}