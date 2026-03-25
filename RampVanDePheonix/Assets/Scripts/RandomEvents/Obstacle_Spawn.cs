using UnityEngine;

public class Obstacle_Spawn : MonoBehaviour
{
    int rockHealth;

    public bool hasSpawned;

    public void Start()
    {
        hasSpawned = true;
        rockHealth = Random.Range(3,9);
    }

    private void FixedUpdate()
    {
        Vector2 targetPos;
        targetPos = new(750, 120);

        transform.position = Vector2.MoveTowards(transform.position, targetPos, 2);

        if (Vector2.Distance(transform.position, targetPos) < 1f)
        {
            BackgroundMovement background = FindFirstObjectByType<BackgroundMovement>();
            BrendaFlip brendaFlip = FindAnyObjectByType<BrendaFlip>();
            brendaFlip.enabled = false;
            background.enabled = false;
        }
    }

    public void ClickedRock()
    {
        rockHealth--;

        if (rockHealth < 0)
        {
            BackgroundMovement background = FindFirstObjectByType<BackgroundMovement>();
            background.enabled = true;
            BrendaFlip brendaFlip = FindAnyObjectByType<BrendaFlip>();
            brendaFlip.enabled = true;
            hasSpawned = false;
            Destroy(gameObject);
        }
    }
}
