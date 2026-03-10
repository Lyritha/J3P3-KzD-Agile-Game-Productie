using System.Collections;
using UnityEngine;

public class EventStateManagerTutorial : EventStateManager
{

    // disable event setting without breaking references
    public override void SetState(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.Walking:
                break;
            case State.Event:
                break;
            case State.FinishEvent:
                FinishEventState();
                break;
            case State.Minigame:
                break;
            case State.Exit:
                break;
        }
    }

    public void TriggerEvent()
    {
        Event randomEvent = faseManager.GetRandomEvent();
        eventVisualiser.gameObject.SetActive(true);
        eventVisualiser.ShowEvent(randomEvent);
    }

    protected override void FinishEventState()
    {
        Debug.Log("apply stat changes");
        faseManager.AddProgress();
        CharacterListDisplay.Instance.UpdateCharacters();
        MainGameTutorial.Instance.StartStep();
    }
}
