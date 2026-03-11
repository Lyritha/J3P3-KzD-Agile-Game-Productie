using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterFood : MonoBehaviour
{
    [SerializeField]
    private Image[] hungerItems;
    [SerializeField]
    private RectTransform foodText;
    [SerializeField]
    private RectTransform starvingText;

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
        // when the character runs out of food, 50% chance to die, can be changed to something more complex later
        bool shouldDie = currentFood <= 0 && Random.value < 0.5f;
        if (shouldDie)
        {
            foodText.gameObject.SetActive(false);
            starvingText.gameObject.SetActive(false);
            parent.Die("Lack of food");
        }

        currentFood = Mathf.Max(currentFood - 1, 0);
        UpdateFoodUI(shouldDie);
    }

    public bool TryGiveFood()
    {
        if (currentFood < maxFood && foodStorage.GivingFood && foodStorage.RemoveFood())
        {
            currentFood++;
            UpdateFoodUI();
            return true;
        }

        return false;
    }

    private void UpdateFoodUI(bool disableUI = false)
    {
        if (disableUI) return;

        bool isStarving = currentFood <= 0;
        foodText.gameObject.SetActive(!isStarving);
        starvingText.gameObject.SetActive(isStarving);


        for (int i = 0; i < hungerItems.Length; i++)
            hungerItems[i].color = i < currentFood ? fullColor : emptyColor;
    }
}
