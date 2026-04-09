using UnityEngine;

public class RandomEventsTut : RandomEventManager
{
    // do nothing to avoid random events from happening during the tutorial
    public override void StartEventLoop() { }

    // do nothing to avoid random events from happening during the tutorial
    public override void EndEventLoop() { }

    public void ManualSpawnObstakel() => Invoke(nameof(SpawnObstacle), 2);
    public void ManualSpawnThief() => Invoke(nameof(SpawnThief), 1);
    public void ManualSpawnLoot() => Invoke(nameof(SpawnLoot), 0);



    public override void SpawnThief()
    {
        Eventhappening = true;

        int amount = Random.Range(1, 4);
        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPos = new(Random.Range(-100, -20), Random.Range(10, 80));
            Stealing_enemy obj = Instantiate(stealPrefabA, spawnPos, Quaternion.identity, rectTransform);
            obj.Init(this);
        }
    }
    public override void SpawnLoot()
    {
        Eventhappening = true;

        Vector2 spawnPos = new(Random.Range(-100, -20), Random.Range(10, 80));
        Loot_Spawn obj = Instantiate(lootPrefabA, spawnPos, Quaternion.identity, rectTransform);
        obj.Init(rectTransform, this);
    }
    public override void SpawnObstacle()
    {
        Eventhappening = true;

        Vector2 spawnPos = new(-100, 60);
        Obstacle_Spawn obj = Instantiate(obstaclePrefabA, spawnPos, Quaternion.identity, rectTransform);
        obj.Init(this);
    }
}
