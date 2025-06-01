using UnityEngine;

public class damManager : MonoBehaviour
{
    public float buildPoints = 30; // Set initial build points in Inspector
    public float maxBuildPoints = 30; // Set max build points in Inspector

    public bool IsBuilt => buildPoints <= 0;

    void Start()
    {
    }

    void Update()
    {
        // Clamp buildPoints to never be less than 0
        if (buildPoints < 0)
            buildPoints = 0;
    }

    // Call this method to reduce build points (e.g., by a beaver)
    public void Build(float amount)
    {
        buildPoints -= amount;
        if (buildPoints < 0) buildPoints = 0;
    }
}
