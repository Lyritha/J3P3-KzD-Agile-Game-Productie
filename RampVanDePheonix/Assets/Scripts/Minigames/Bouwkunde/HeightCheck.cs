using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeightCheck : MonoBehaviour
{
    public List<GameObject> spawnedBricks = new List<GameObject>();


    [Header("UI Elements")]
    [SerializeField] TMP_Text heightIndicator;

    float highest = 0;

    void Update()
    {
        CheckHeight();
    }

    void CheckHeight()
    {
        highest = 0;
        foreach (GameObject brick in spawnedBricks)
        {
            if (brick.transform.position.y > highest)
            {
                highest = brick.transform.position.y;
            }                                
        }

        heightIndicator.text = Convert.ToString(Math.Round(highest));
    }
}
