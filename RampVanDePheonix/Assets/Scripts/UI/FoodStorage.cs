using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodStorage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static FoodStorage Instance { get; private set; }

    [SerializeField]
    private TMP_Text foodText;
    [SerializeField]
    private Image foodBorder;
    [SerializeField]
    private int startingFood = 3;

    [SerializeField]
    private List<Image> foodIcons = new();

    public int MaxFood { get; private set; } = 5;
    public int CurrentFood { get; private set; } = 0;

    private static readonly Color EmptyColor = Color.black;
    private static readonly Color FilledColor = Color.red;

    private static readonly Color borderColor = new Color32(0x8C, 0x73, 0x49, 0xFF);
    private static readonly Color highlightedBorderColor = Color.red;

    private static readonly string baseFoodText = "Eten opslag";
    private static readonly string foodAvailableHover = "Eten geven";
    private static readonly string foodUnavailableHover = "Geen eten";

    public bool GivingFood { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        MaxFood = foodIcons.Count;
        CurrentFood = startingFood;
        UpdateDisplay();
    }

    /// <summary>
    /// Adds food to the storage.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>true if at least 1 food can be added</returns>
    public bool TryAddFood(int amount)
    {
        if (CurrentFood >= MaxFood) return false;

        CurrentFood += amount;
        UpdateDisplay();

        return true;
    }

    /// <summary>
    /// Removes food to the storage.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>true if at least 1 food can be added</returns>
    public bool TryRemoveFood(int amount)
    {
        if (CurrentFood <= MaxFood) return false;

        CurrentFood -= amount;
        UpdateDisplay();

        return true;
    }

    public bool RemoveFood()
    {
        if (CurrentFood <= 0) return false;
        CurrentFood--;
        UpdateDisplay();

        return true;
    }

    [ContextMenu("Update Display")]
    public void UpdateDisplay()
    {
        for (int i = 0; i < foodIcons.Count; i++)
            foodIcons[i].color = i < CurrentFood ? FilledColor : EmptyColor;
    }

    public void ToggleGivingFood()
    {
        GivingFood = !GivingFood;
        foodBorder.color = GivingFood ? highlightedBorderColor : borderColor;
    }



    public void OnPointerEnter(PointerEventData eventData) => foodText.text = CurrentFood > 0 ? foodAvailableHover : foodUnavailableHover;

    public void OnPointerExit(PointerEventData eventData) => foodText.text = baseFoodText;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CurrentFood > 0 || GivingFood)
            ToggleGivingFood();
    }
}
