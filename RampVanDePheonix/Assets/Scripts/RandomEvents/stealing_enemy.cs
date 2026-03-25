using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class stealing_enemy : MonoBehaviour
{
    GameObject brendaTarget;
    bool hasStolen;

    void Start()
    {
        brendaTarget = FindAnyObjectByType<Player>().gameObject;
    }

    void FixedUpdate()
    {
        Vector2 targetPos;
        if (!hasStolen)
        {
            targetPos = brendaTarget.transform.position;
            targetPos.y -= 80;
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

        transform.position = Vector2.MoveTowards(transform.position, targetPos, 2);

        if (Vector3.Distance(transform.position,targetPos)<50 && !hasStolen)
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
