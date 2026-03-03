using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodStorage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text foodText;
    [SerializeField]
    private Image foodBorder;
    [SerializeField]
    private int startingFood = 3;

    [SerializeField]
    private List<Image> foodIcons = new();

    private int maxFood = 5;
    private int currentFood = 0;

    private static readonly Color EmptyColor = Color.black;
    private static readonly Color FilledColor = Color.red;

    private static readonly Color borderColor = new Color32(0x8C, 0x73, 0x49, 0xFF);
    private static readonly Color highlightedBorderColor = Color.red;

    private static readonly string baseFoodText = "Eten opslag";
    private static readonly string foodAvailableHover = "Eten geven";
    private static readonly string foodUnavailableHover = "Geen eten";

    public bool GivingFood { get; private set; } = false;

    void Start()
    {
        maxFood = foodIcons.Count;
        currentFood = startingFood;
        UpdateDisplay();
    }

    public void AddFood()
    {
        if (currentFood >= maxFood) return;
        currentFood++;
        UpdateDisplay();
    }

    public void RemoveFood()
    {
        if (currentFood <= 0) return;
        currentFood--;
        UpdateDisplay();
    }

    [ContextMenu("Update Display")]
    public void UpdateDisplay()
    {
        for (int i = 0; i < foodIcons.Count; i++)
            foodIcons[i].color = i < currentFood ? FilledColor : EmptyColor;
    }

    public void ToggleGivingFood()
    {
        GivingFood = !GivingFood;
        foodBorder.color = GivingFood ? highlightedBorderColor : borderColor;
    }



    public void OnPointerEnter(PointerEventData eventData) => foodText.text = currentFood > 0 ? foodAvailableHover : foodUnavailableHover;

    public void OnPointerExit(PointerEventData eventData) => foodText.text = baseFoodText;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentFood > 0 || GivingFood)
            ToggleGivingFood();
    }
}
