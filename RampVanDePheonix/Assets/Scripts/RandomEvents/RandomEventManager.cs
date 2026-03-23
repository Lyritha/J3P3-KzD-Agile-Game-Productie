using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class RandomEventManager : MonoBehaviour
{
    [SerializeField] stealing_enemy stealingEnemyScript;

    IEnumerator IntervalTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(20f, 40f));
            TriggerRandomEvent();
        }
    }

    void TriggerRandomEvent()
    {
        int randomEvent = Random.Range(0, 1);

        if (randomEvent == 0)
        {
            stealingEnemyScript.SpawnThief();
        }
    }

    private void Start()
    {
        StartCoroutine(IntervalTimer());
    }
}
