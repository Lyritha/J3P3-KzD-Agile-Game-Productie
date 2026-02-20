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

        heightIndicator.text = Convert.ToString(Math.Round(highest));
    }

    void MoveIndicator()
    {
        indicator.transform.position = new Vector2(0,highest);
    }
}
