using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private int foodPrice = 1;
    [SerializeField]
    private int foodGetAmount = 5;

    public void BuyFood()
    {
        if (!CharacterListDisplay.Instance.TryGetSelectedCharacter(out Character character))
        {
            Debug.Log("No character selected to buy food with");
            return;
        }

        if (character.Personage.baseKapitaal <= 0)
        {
            Debug.Log("Cannot buy food: character has no capital");
            return;
        }

        if (!FoodStorage.Instance.TryAddFood(5))
        {
            Debug.Log("Cannot buy food: storage is full");
            return;
        }

        character.Personage.baseKapitaal--;
        character.Personage.NotifyChanged();
        Debug.Log("Bought food");
    }

    public void CloseMenu()
    {
        if (TryGetComponent(out PopupAnim anim))
        {
            anim.CloseAnim();
        }
        else
        {
            gameObject.SetActive(false);
        }

        EventStateManager.Instance.SetState(State.Minigame);
    }
}
