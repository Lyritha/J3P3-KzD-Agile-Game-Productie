using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stealing_enemy : MonoBehaviour
{
    GameObject brendaTarget;
    bool hasStolen;
    RectTransform parentRect;
    RandomEventManager parent;


    public void Init(RandomEventManager parent)
    {
        this.parent = parent;
        brendaTarget = FindAnyObjectByType<Player>().gameObject;
        parentRect = (RectTransform)transform.parent;
    }

    void FixedUpdate()
    {
        if (!parent.Eventhappening) return;

        Vector2 targetPos;
        if (!hasStolen)
        {
            targetPos = brendaTarget.transform.position;
            targetPos.y -= (parentRect.rect.height / 100) * 15;
        }
        else
        {
            transform.localScale = new(-1,1,1);
            targetPos = new(-50, 20);
            if (Vector3.Distance(transform.position, targetPos) < 10)
            {
                Destroy(gameObject);
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, 10);

        if (Vector3.Distance(transform.position,targetPos) < 50 && !hasStolen)
        {
            FoodStorage.Instance.RemoveFood();
            hasStolen = true;
        }
    }

    public void ClickedRat()
    {
        if (hasStolen)
        {
            FoodStorage.Instance.TryAddFood(1);
        }
        Destroy(gameObject);
    }
}
