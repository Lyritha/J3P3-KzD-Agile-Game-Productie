using UnityEngine;
using UnityEngine.UI;

public class CharacterFood : MonoBehaviour
{
    [SerializeField]
    private Image[] hungerItems;

    private int maxFood = 3;
    private int currentFood = 3;

    private Color fullColor = Color.red;
    private Color emptyColor = Color.black;

    private Character parent;

    public void Initialize(Character parent)
    {
        int food = hungerItems.Length;
        maxFood = food;
        currentFood = food;
        UpdateFoodUI();

        this.parent = parent;
    }

    [ContextMenu("Consume Food")]
    public void ConsumeFood()
    {
        if (currentFood > 0)
        {
            currentFood--;
            UpdateFoodUI();
        }

        // when the character runs out of food, 50% chance to die, can be changed to something more complex later
        bool shouldDie = currentFood <= 0 && Random.value < 0.5f;
        if (shouldDie) parent.Die("Lack of food");
    }

    [ContextMenu("Restore Food")]
    public void RestoreFood()
    {
        if (currentFood < maxFood)
        {
            currentFood++;
            UpdateFoodUI();
        }
    }

    private void UpdateFoodUI()
    {
        for (int i = 0; i < hungerItems.Length; i++)
        {
            bool isFull = i < currentFood;
            hungerItems[i].color = isFull ? fullColor : emptyColor;
        }
    }
}
