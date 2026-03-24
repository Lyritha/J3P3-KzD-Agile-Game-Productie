using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class RandomEventManager : MonoBehaviour
{
    [SerializeField] GameObject ratPrefab;
    [SerializeField] GameObject lootPrefab;
    [SerializeField] RectTransform rectTransform;
    bool eventhappening;

    IEnumerator IntervalTimer()
    {
        while (eventhappening)
        {
            yield return new WaitForSeconds(Random.Range(2f, 10f));
            TriggerRandomEvent();
        }
    }

    void TriggerRandomEvent()
    {
        int randomEvent = Random.Range(0, 2);

        if (randomEvent == 0)
        {
            SpawnThief();
        }
        else if (randomEvent == 1)
        {
            SpawnLoot();
        }
    }

    private void Start()
    {
        eventhappening = true;
        StartCoroutine(IntervalTimer());
    }

    public void StartEventLoop()
    {
        eventhappening = true;
        StartCoroutine(IntervalTimer());
    }

    public void EndEventLoop()
    {
        eventhappening = false;
    }

    public void SpawnThief()
    {
        int amount = Random.Range(1, 4);

        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPos = new Vector2(Random.Range(-100, -20), Random.Range(10, 80));

            Instantiate(ratPrefab, spawnPos, Quaternion.identity, rectTransform);
        }
    }

    public void SpawnLoot()
    {
        Vector2 spawnPos = new Vector2(Random.Range(-100, -20), Random.Range(10, 80));

        Instantiate(lootPrefab, spawnPos, Quaternion.identity, rectTransform);
    }
}
