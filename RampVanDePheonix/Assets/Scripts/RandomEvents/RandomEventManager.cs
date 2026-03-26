using System.Collections;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    [SerializeField] Stealing_enemy ratPrefab;
    [SerializeField] Obstacle_Spawn obstaclePrefab;
    [SerializeField] Loot_Spawn lootPrefab;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] float minDelay = 2;
    [SerializeField] float maxDelay = 10;

    public bool Eventhappening {  get; private set; }

    private Coroutine coroutine;

    IEnumerator IntervalTimer()
    {
        while (Eventhappening)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            TriggerRandomEvent();
        }
    }

    void TriggerRandomEvent()
    {
        int randomEvent = Random.Range(0, 3);

        if (randomEvent == 0)
        {
            SpawnThief();
        }
        else if (randomEvent == 1)
        {
            SpawnLoot();
        }
        else if (randomEvent == 2)
        {
            SpawnObstacle();
        }
    }

    public void StartEventLoop()
    {
        Eventhappening = true;
        coroutine = StartCoroutine(IntervalTimer());
    }

    public void EndEventLoop()
    {
        Eventhappening = false;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void SpawnThief()
    {
        int amount = Random.Range(1, 4);

        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPos = new(Random.Range(-100, -20), Random.Range(10, 80));

            Stealing_enemy obj = Instantiate(ratPrefab, spawnPos, Quaternion.identity, rectTransform);
            obj.Init(this);
        }
    }

    public void SpawnLoot()
    {
        Vector2 spawnPos = new(Random.Range(-100, -20), Random.Range(10, 80));

        Loot_Spawn obj = Instantiate(lootPrefab, spawnPos, Quaternion.identity, rectTransform);
        obj.Init(rectTransform, this);
    }

    public void SpawnObstacle()
    {
        Obstacle_Spawn obstacle = FindAnyObjectByType<Obstacle_Spawn>();
        
        if (obstacle == null)
        {
            Vector2 spawnPos = new(-100, 120);
            Obstacle_Spawn obj = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity, rectTransform);
            obj.Init(this);
        }
        else return;

    }
}
