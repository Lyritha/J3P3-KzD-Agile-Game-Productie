using MyBox;
using UnityEngine;

public class Loot_Spawn : MonoBehaviour
{
    Vector2 targetPos;

    public void Init(RectTransform parentRect)
    {
        float y = Random.Range(10, 50) - (parentRect.rect.height / 2);
        float rightEdge = parentRect.rect.width / 2;

        RectTransform rt = (RectTransform)transform;
        targetPos = new Vector2(rightEdge + rt.rect.width, y);
    }

    void FixedUpdate()
    {
        RectTransform rt = (RectTransform)transform;
        rt.anchoredPosition = Vector2.MoveTowards( rt.anchoredPosition, targetPos, 200f * Time.fixedDeltaTime);

        if (Vector2.Distance(rt.anchoredPosition, targetPos) < 1f)
        {
            Destroy(gameObject);
        }
    }

    public void ClickedLoot()
    {
        int randomValue = Random.Range(0, 2);

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
