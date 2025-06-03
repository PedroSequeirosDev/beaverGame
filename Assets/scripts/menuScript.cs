using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    public fade screenfade;
    public void StartGame()
    {
        StartCoroutine(FadeNextScene());
    }

    private IEnumerator FadeNextScene()
    {
        yield return screenfade.FadeIn();
        Time.timeScale = 1; 
        SceneManager.LoadScene("BeaverFever");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
