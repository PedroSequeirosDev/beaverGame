using UnityEngine;
using UnityEngine.UI;

public class sliderManager : MonoBehaviour
{
    public damManager damManager;
    public Slider slider;

    void Start()
    {
        slider.maxValue = damManager.maxBuildPoints;
        slider.minValue = 0;
    }

    void Update()
    {
        slider.value = damManager.maxBuildPoints - damManager.buildPoints;   
    }
}
