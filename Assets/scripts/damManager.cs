using UnityEngine;
using UnityEngine.SceneManagement;

public class damManager : MonoBehaviour
{
    public float buildPoints = 1000; // Set initial build points in Inspector
    public float maxBuildPoints = 1000; // Set max build points in Inspector

    public bool IsBuilt => buildPoints <= 0;
    public bool panelactive = false;

    [SerializeField] private GameObject damGameOver;


    void Update()
    {
        // Clamp buildPoints to never be less than 0
        if (buildPoints < 0)
        {
            buildPoints = 0;
        }

        if (buildPoints >= maxBuildPoints)
        {
            if (panelactive == false)
            {


                Debug.Log(" Oh no! The dam has been destroyed!");
                // Show game over screen
                panelactive = true;
                damGameOver.SetActive(true);
                Time.timeScale = 0;

            }
        }
    }

    // Call this method to reduce build points (e.g., by a beaver)
    public void Build(float amount)
    {
        buildPoints -= amount;
        if (buildPoints < 0) buildPoints = 0;
    }
    
    
    public void QuitToMenu()
    {
        Debug.Log("Returning to menu");
        Time.timeScale = 1;
        SceneManager.LoadScene("mainMenu");

    }
}
