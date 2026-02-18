using UnityEngine;
using UnityEngine.UI;

public class FishingManager : MonoBehaviour
{
    public RectTransform fish;
    public RectTransform playerBar;
    public Slider catchSlider;

    public float catchSpeed = 0.5f;
    public float loseSpeed = 0.7f;

    void Update()
    {
        float distance = Mathf.Abs(fish.anchoredPosition.y - playerBar.anchoredPosition.y);

        if (distance < 40f)
        {
            catchSlider.value += catchSpeed * Time.deltaTime;
        }
        else
        {
            catchSlider.value -= loseSpeed * Time.deltaTime;
        }

        catchSlider.value = Mathf.Clamp01(catchSlider.value);

        if (catchSlider.value >= 1f)
        {
            Debug.Log("VIS GEVANGEN");
        }
    }
}
