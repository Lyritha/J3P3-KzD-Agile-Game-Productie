using System;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float amountOfSeconds;
    public static Action OnCountDone;
    [SerializeField] TMP_Text timerText;
    void Update()
    {
        Countdown();
    }

    void Countdown()
    {
        amountOfSeconds -= Time.deltaTime;
        if (amountOfSeconds <= 0)
        {
            OnCountDone?.Invoke();
        } 
        timerText.text = Convert.ToString(Math.Round(amountOfSeconds));
    }
}
