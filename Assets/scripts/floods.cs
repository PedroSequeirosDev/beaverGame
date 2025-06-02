using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class floods : MonoBehaviour
{
   [SerializeField] float floodTimer = 120; //variable to alter for floods
   [SerializeField] float floodInterval = 120;
    [SerializeField] float floodDamage = 200;

    [SerializeField] private float floodWarningTime = 40f;

    [SerializeField] float intervalDecrease = 20f;

    [SerializeField] float damageIncrease = 10f;

    [SerializeField] float minInterval = 40f;

    // each second the floodTimer will decrease by 1

    // when floodTimer reaches 0, it will call the flood function
    void Update()
    {
        floodTimer -= Time.deltaTime;

        if (floodTimer <= 0)
        {
            Flood();
            floodInterval = Mathf.Max(floodInterval - intervalDecrease, minInterval); // Decrease the interval but not below minInterval
            floodDamage += damageIncrease; // Increase the flood damage
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

        Debug.Log("Flooding! Dam build points reduced by " + floodDamage);
    }
}
