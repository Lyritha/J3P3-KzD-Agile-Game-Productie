using UnityEngine;

public class Obstacle_Spawn : MonoBehaviour
{
    int rockHealth;

    public bool hasSpawned;
    public static bool isBlocking;
    private bool startedBlocking;

    RandomEventManager parent;
    RectTransform parentRect;

    public void Init(RandomEventManager parent)
    {
        this.parent = parent;
        hasSpawned = true;
        rockHealth = Random.Range(3, 9);
        parentRect = (RectTransform)transform.parent;
    }

    private void FixedUpdate()
    {
        if (!parent.Eventhappening) return;

        Vector2 targetPos;
        targetPos = new((parentRect.rect.width / 100) * 20, (parentRect.rect.height / 100) * 7);

        transform.position = Vector2.MoveTowards(transform.position, targetPos, 7);

        if (Vector2.Distance(transform.position, targetPos) < (parentRect.rect.width / 100) * 20)
        {
            BackgroundMovement background = FindFirstObjectByType<BackgroundMovement>();
            background.PauseBackground();
            startedBlocking = true;
        }

        if (Vector2.Distance(transform.position, targetPos) < 1f)
        {
            isBlocking = true;
        }
    }

    public void ClickedRock()
    {
        rockHealth--;

        if (rockHealth < 0)
            RemoveRock();
    }

    public void RemoveRock()
    {
        if (startedBlocking)
        {
            BackgroundMovement background = FindFirstObjectByType<BackgroundMovement>();
            background.StartBackground();
        }

        startedBlocking = false;
        isBlocking = false;
        hasSpawned = false;
        Destroy(gameObject);
    }
}
