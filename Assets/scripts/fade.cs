using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class fade : MonoBehaviour
{
    [SerializeField] public float fadeDuration = 2f;
    private Image fader;

    void Start()
    {
        fader = GetComponent<Image>();
    }

    public IEnumerator FadeIn()
    {
        fader.raycastTarget = true;
        float timer = 0f;
        Color color = fader.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, timer / fadeDuration);
            fader.color = color;
            yield return null;
        }
        color.a = 1;
        fader.color = color;
        
    }
}
