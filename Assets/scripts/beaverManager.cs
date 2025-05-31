using UnityEngine;
using System.Linq;

public class beaverManager : MonoBehaviour
{
    public GameObject beaverPrefab; // Assign in Inspector
    public Transform spawnPointA;   // Assign in Inspector
    public Transform spawnPointB;   // Assign in Inspector

    public int maxPopulation = 0;
    public int beaversPerHouse = 5;

    void Update()
    {
        int houseCount = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Count(obj => obj.CompareTag("House"));

        maxPopulation = houseCount * beaversPerHouse;

        int currentBeavers = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
            .Count(obj => obj.CompareTag("Beaver"));

        while (currentBeavers < maxPopulation)
        {
            Instantiate(beaverPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            currentBeavers++;
        }
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