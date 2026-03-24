using MyBox;
using UnityEngine;

public class Loot_Spawn : MonoBehaviour
{
    RectTransform rectTransform;

    public void Init(RectTransform rectTransform)
    {
        this.rectTransform = rectTransform;
    }

    void FixedUpdate()
    {
        Vector2 targetPos;
        targetPos = new(rectTransform.rect.width, Random.Range(10, 80));

        transform.position = Vector2.MoveTowards(transform.position, targetPos, 2);

        if (Vector3.Distance(transform.position, targetPos) < 10)
        {
            Destroy(gameObject);
        }
    }

    public void ClickedLoot()
    {
        int randomValue = Random.Range(0,2);

        if (randomValue == 0)
        {
            FoodStorage.Instance.TryAddFood(1);
        }

        if (randomValue == 1)
        {
            var randomCharacter = CharacterListDisplay.Instance.Characters.GetRandom();
            randomCharacter.Personage.baseKapitaal += 1;

            randomCharacter.Personage.NotifyChanged();
        }
        Destroy(gameObject);
    }
}
