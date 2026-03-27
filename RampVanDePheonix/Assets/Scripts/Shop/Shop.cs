using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private int foodPrice = 1;
    [SerializeField]
    private int foodGetAmount = 5;

    [SerializeField]
    private Image buttonImg;

    bool canTriggerButtonChangeColor = true;


    public void BuyFood()
    {
        if (!CharacterListDisplay.Instance.TryGetSelectedCharacter(out Character character))
        {
            if (canTriggerButtonChangeColor)
            {
                StartCoroutine(TemporaryChangeBackgroundColor(Color.red, 0.5f));
            }
            return;
        }

        if (character.Personage.baseKapitaal < foodPrice)
        {
            Debug.Log("Cannot buy food: character has no capital");
            return;
        }

        if (!FoodStorage.Instance.TryAddFood(foodGetAmount))
        {
            Debug.Log("Cannot buy food: storage is full");
            return;
        }

        character.Personage.baseKapitaal -= foodPrice;
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

    IEnumerator TemporaryChangeBackgroundColor(Color changeToColor, float seconds)
    {
        int changeAmount = 7;
        canTriggerButtonChangeColor = false;
        Color originalColor = buttonImg.color;
        for (int i = 0; i < changeAmount; i++)
        {
            if (i % 2 == 0)
            {
                buttonImg.color = changeToColor;
            }
            else
            {
                buttonImg.color = originalColor;
            }
            yield return new WaitForSeconds(seconds / changeAmount);
        }
        buttonImg.color = originalColor;
        canTriggerButtonChangeColor = true;
        yield return new WaitForSeconds(0.1f);
    }
}
