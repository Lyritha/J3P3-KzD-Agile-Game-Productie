using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] FaseManager faseManager;

    public QuestionScriptable GetRandomEvent()
    {
        var events = faseManager.currentEvents;

        int randomIndex = Random.Range(0, events.Count);
        return events[randomIndex];
    }
}