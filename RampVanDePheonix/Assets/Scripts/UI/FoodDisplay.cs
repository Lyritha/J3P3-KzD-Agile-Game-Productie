using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodDisplay : MonoBehaviour
{
    [SerializeField]
    private List<Image> foodIcons = new();

    [SerializeField]
    private int currentFood = 0;

    private static readonly Color EmptyColor = Color.black;
    private static readonly Color FilledColor = Color.red;

    void Start()
    {
        foreach (Image foodIcon in foodIcons) foodIcon.color = EmptyColor;
    }

    public void SetFood(int food)
    {
        currentFood = food;
        UpdateDisplay();
    }

    [ContextMenu("Update Display")]
    public void UpdateDisplay()
    {
        for (int i = 0; i < foodIcons.Count; i++)
            foodIcons[i].color = i < currentFood ? FilledColor : EmptyColor;
    }
}
