using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseManager;
    bool isPaused = false;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            Debug.Log("Pause");
            isPaused = true;
            pauseManager.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        pauseManager.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;

    }

    public void QuitToMenu()
    {
        Debug.Log("Returning to menu");
        Time.timeScale = 1;
        SceneManager.LoadScene("mainMenu");
    
    }

    
        
    
}
