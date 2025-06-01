using UnityEngine;
using UnityEngine.SceneManagement;

public class floods : MonoBehaviour
{
   [SerializeField] float floodTimer = 120; //variable to alter for floods
   [SerializeField] float floodInterval = 120;

    [SerializeField] float floodDamage = 200;

    // each second the floodTimer will decrease by 1

    // when floodTimer reaches 0, it will call the flood function
    void Update()
    {
        floodTimer -= Time.deltaTime;

        if (floodTimer <= 0)
        {
            Flood();
            floodTimer = floodInterval; // Reset the timer
        }

    }

    void Flood()
    {
        damManager dam = FindFirstObjectByType<damManager>();
        if (dam == null)
        {
            Debug.LogError("No damManager found!");
            return;
        }
        
        dam.buildPoints += floodDamage;

        if (dam.buildPoints >= dam.maxBuildPoints)
        {
            Debug.Log("Dam has been destroyed!");
            SceneManager.LoadScene("mainMenu");
        }
        

        Debug.Log("Flooding! Dam build points reduced by " + floodDamage);
    }
}
