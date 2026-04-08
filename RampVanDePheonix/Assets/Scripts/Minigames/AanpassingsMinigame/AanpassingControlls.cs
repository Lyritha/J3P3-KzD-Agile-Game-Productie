using TMPro;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class AanpassingControlls : MonoBehaviour
{
    [SerializeField] Timer timer;
    [SerializeField] RectTransform cartImage;
    [SerializeField] TMP_Text progressText;
    [SerializeField] TMP_Text stateText;
    [SerializeField] float drainSpeed = 2f;
    [SerializeField] int penalty = 10;

    [SerializeField] AudioClip incorrectJingle;
    AudioSource source;

    private bool canInput = true;
    private int score;
    private bool backNForth = false;

    private float progressCounter;

    void Start()
    {
        source = GetComponent<AudioSource>();
        source.clip = incorrectJingle;
        MinigameFinished.Instance.ShowScore(100);
        progressCounter = 0f;
        backNForth = false;
    }

    void Update()
    {
        if (progressCounter >= 99f)
        {
            canInput = false;
            timer.amountOfSeconds = 0f;
        }

        else if (progressCounter >= 90f)
        {
            cartImage.anchoredPosition = new Vector2(-425, -90);
            cartImage.rotation = Quaternion.Euler(0, 0, 0);
            stateText.text = "Los";
            score = 3;
            MinigameFinished.Instance.ShowScore(score);
        }

        else if (progressCounter >= 60f)
        {
            cartImage.anchoredPosition = new Vector2(-425, -150);
            cartImage.rotation = Quaternion.Euler(0, 0, -10);
            stateText.text = "Deels los";
            System.Console.Beep();
            score = 2; ;
            MinigameFinished.Instance.ShowScore(score);
        }

        else if (progressCounter >= 30f)
        {
            cartImage.anchoredPosition = new Vector2(-425, -200);
            cartImage.rotation = Quaternion.Euler(0, 0, -15);
     
            stateText.text = "Deels vast";
            score = 1;
            MinigameFinished.Instance.ShowScore(score);
        }

        else if (progressCounter >= 0f)
        {
            cartImage.anchoredPosition = new Vector2(-425, -250);
            cartImage.rotation = Quaternion.Euler(0, 0, -20);
            stateText.text = "Volledig vast";
            score = 0;
            MinigameFinished.Instance.ShowScore(score);
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!canInput)
            {
                return;
            }

            if (!backNForth)
            {
                source.Play();
                progressCounter -= penalty;
            }
            else
            {
                progressCounter += 1;
                backNForth = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!canInput)
            {
                return;
            }
            if (backNForth)
            {
                source.Play();
                progressCounter -= penalty;
            }
            else
            {
                progressCounter += 1;
                backNForth = true;
            }
        }

        progressCounter = Mathf.Clamp(progressCounter, 0f, 100f);

        if (progressCounter > 0f)
        {
            progressCounter -= drainSpeed * Time.deltaTime;
            progressCounter = Mathf.Max(progressCounter, 0f);
        }

        progressText.text = Mathf.RoundToInt(progressCounter) + "%";
    }
}