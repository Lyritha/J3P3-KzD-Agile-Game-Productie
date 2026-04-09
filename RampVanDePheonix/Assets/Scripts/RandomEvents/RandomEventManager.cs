using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    [SerializeField] protected PhaseManager phaseManager;

    [Header("Achterhoek evenementen")]
    [SerializeField] protected Stealing_enemy stealPrefabA;
    [SerializeField] protected Obstacle_Spawn obstaclePrefabA;
    [SerializeField] protected Loot_Spawn lootPrefabA;

    [Header("Boot evenementen")]
    [SerializeField] Stealing_enemy stealPrefabB;
    [SerializeField] Obstacle_Spawn obstaclePrefabB;
    [SerializeField] Loot_Spawn lootPrefabB;

    [Header("Amerika evenementen")]
    [SerializeField] Stealing_enemy stealPrefabAm;
    [SerializeField] Obstacle_Spawn obstaclePrefabAm;
    [SerializeField] Loot_Spawn lootPrefabAm;

    [Header("Spawn settings")]
    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] float minDelay = 2;
    [SerializeField] float maxDelay = 10;

    public bool Eventhappening;

    private List<Stealing_enemy> activeEnemies = new List<Stealing_enemy>();
    private List<Obstacle_Spawn> activeObstacles = new List<Obstacle_Spawn>();
    private List<Loot_Spawn> activeLoots = new List<Loot_Spawn>();

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
        if (Obstacle_Spawn.isBlocking) return;

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

    public virtual void StartEventLoop()
    {
        Eventhappening = true;
        coroutine = StartCoroutine(IntervalTimer());
    }

    public virtual void EndEventLoop()
    {
        Eventhappening = false;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public virtual void SpawnThief()
    {
        int amount = Random.Range(1, 4);

        Stealing_enemy enemy = phaseManager.CurrentFase switch
        {
            Fases.Achterhoek => stealPrefabA,
            Fases.Pheonix => stealPrefabB,
            Fases.Amerika => stealPrefabAm,
            _ => null,
        };

        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPos = new(Random.Range(-100, -20), Random.Range(10, 80));

            Stealing_enemy obj = Instantiate(enemy, spawnPos, Quaternion.identity, rectTransform);
            obj.Init(this);
            activeEnemies.Add(obj);
        }
    }
    public virtual void SpawnLoot()
    {
        Vector2 spawnPos = new(Random.Range(-100, -20), Random.Range(10, 80));

        Loot_Spawn loot = phaseManager.CurrentFase switch
        {
            Fases.Achterhoek => lootPrefabA,
            Fases.Pheonix => lootPrefabB,
            Fases.Amerika => lootPrefabAm,
            _ => null,
        };

        Loot_Spawn obj = Instantiate(loot, spawnPos, Quaternion.identity, rectTransform);
        obj.Init(rectTransform, this);
        activeLoots.Add(obj);
    }
    public virtual void SpawnObstacle()
    {
        Obstacle_Spawn obstacle = FindAnyObjectByType<Obstacle_Spawn>();
        if (obstacle != null) return;

        Obstacle_Spawn obstacleSpawn = phaseManager.CurrentFase switch
        {
            Fases.Achterhoek => obstaclePrefabA,
            Fases.Pheonix => obstaclePrefabB,
            Fases.Amerika => obstaclePrefabAm,
            _ => null,
        };

        Vector2 spawnPos = new(-100, 60);
        Obstacle_Spawn obj = Instantiate(obstacleSpawn, spawnPos, Quaternion.identity, rectTransform);
        obj.Init(this);
        activeObstacles.Add(obj);
    }

    public void ClearAllEvents()
    {
        foreach (Stealing_enemy enemy in activeEnemies)
        {
            if (enemy != null) Destroy(enemy.gameObject);
        }

        foreach (Obstacle_Spawn obstacle in activeObstacles)
        {
            if (obstacle != null) obstacle.RemoveRock();
        }

        foreach (Loot_Spawn loot in activeLoots)
        {
            if (loot != null) Destroy(loot.gameObject);
        }

        activeEnemies.Clear();
        activeObstacles.Clear();
        activeLoots.Clear();

    }
}
