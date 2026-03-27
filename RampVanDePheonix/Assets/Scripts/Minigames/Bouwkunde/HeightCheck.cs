using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeightCheck : MonoBehaviour
{
    public List<GameObject> spawnedBricks = new List<GameObject>();

    [Header("UI Elements")]
    [SerializeField] TMP_Text heightIndicator;

    [Header("HeightIndicator")]
    [SerializeField] GameObject indicator;


    public int highestRounded = 0;
    Vector3 highest;


    void Update()
    {
        CheckHeight();
        MoveIndicator();
    }

    void CheckHeight()
    {
        highest = new(1, 1, 0);
        foreach (GameObject brick in spawnedBricks)
        {
            if (brick != null && brick.transform.position.y > highest.y)
            {
                highest = brick.transform.position;
            }                                
        }

        highestRounded = (int)Math.Round(highest.y);
        if (MinigameFinished.Instance != null) MinigameFinished.Instance.ShowScore(highestRounded);
        heightIndicator.text = Convert.ToString(highestRounded);
    }

    void MoveIndicator()
    {
        indicator.transform.position = Vector3.Lerp(indicator.transform.position, new Vector2(0,highest.y), 1f * Time.deltaTime);
        //indicator.transform.position = new Vector2(0,highest);
    }


}
