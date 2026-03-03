using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterFood : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image[] hungerItems;

    private int maxFood = 3;
    private int currentFood;

    private Color fullColor = Color.red;
    private Color emptyColor = Color.black;

    private Character parent;
    private FoodStorage foodStorage;

    public void Initialize(Character parent)
    {
        foodStorage = FindFirstObjectByType<FoodStorage>();

        maxFood = hungerItems.Length;
        currentFood = maxFood;
        UpdateFoodUI();

        this.parent = parent;
    }

    [ContextMenu("Consume Food")]
    public void EatFood()
    {
        currentFood = Mathf.Max(currentFood - 1, 0);
        UpdateFoodUI();

        // when the character runs out of food, 50% chance to die, can be changed to something more complex later
        bool shouldDie = currentFood <= 0 && Random.value < 0.5f;
        if (shouldDie) parent.Die("Lack of food");
    }

    public void GiveFood()
    {
        currentFood = Mathf.Min(currentFood + 1, maxFood);
        UpdateFoodUI();
    }

    private void UpdateFoodUI()
    {
        for (int i = 0; i < hungerItems.Length; i++)
            hungerItems[i].color = i < currentFood ? fullColor : emptyColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentFood < maxFood && foodStorage.GivingFood)
        {
            GiveFood();
            foodStorage.RemoveFood();
            foodStorage.ToggleGivingFood();
        }
    }
}
