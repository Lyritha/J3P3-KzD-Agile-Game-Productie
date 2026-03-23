using UnityEngine;

public class stealing_enemy : MonoBehaviour
{
    GameObject brendaTarget;

    [SerializeField] GameObject ratPrefab;
    [SerializeField] bool isSpawner = false;
    [SerializeField] RectTransform rectTransform;

    bool hasStolen;

    void Start()
    {
        brendaTarget = FindAnyObjectByType<Player>().gameObject;
        if (isSpawner)
        {
            SpawnThief();
        }
    }

    void FixedUpdate()
    {
        if (isSpawner) return;


        Vector2 targetPos;
        if (!hasStolen)
        {
            targetPos = brendaTarget.transform.position;
            targetPos.y -= 80;
        }
        else
        {
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

        if (hasStolen)
        {

        }
    }

    public void SpawnThief()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(-100, -20),
            Random.Range(10, 80)
        );

        Instantiate(ratPrefab, spawnPos, Quaternion.identity, rectTransform);

        int amount = Random.Range(1, 4);

        for (int i = 0; i < amount; i++)
        {
            Invoke(nameof(SpawnThief), Random.Range(5f, 15f));
        }
    }
}
