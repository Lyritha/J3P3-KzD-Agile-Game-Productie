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
    float highest = 0;

    void Update()
    {
        CheckHeight();
        MoveIndicator();
    }

    void CheckHeight()
    {
        highest = 0;
        foreach (GameObject brick in spawnedBricks)
        {
            if (brick != null && brick.transform.position.y > highest)
            {
                highest = brick.transform.position.y;
            }                                
        }

        highestRounded = (int)Math.Round(highest);
        MinigameFinished.Instance.ShowScore(highestRounded);
        heightIndicator.text = Convert.ToString(highestRounded);
    }

    void MoveIndicator()
    {
        indicator.transform.position = new Vector2(0,highest);
    }
}
