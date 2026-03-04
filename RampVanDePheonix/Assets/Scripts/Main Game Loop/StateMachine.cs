using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;

public enum State{
    Walking,
    Event, 
    FinishEvent,
    Minigame,
    Exit
}

public class StateMachine : MonoBehaviour
{
    State state;

    [SerializeField] EventManager eventManager;

    private void Start()
    {
        SetState(State.Walking);
    }

    void SetState(State newState)
    {
        state = newState;

        switch (state)
        {
            case State.Walking:
                StartCoroutine(WalkingState(10));
                break;
            case State.Event:
                EventState();
                break;
            case State.FinishEvent:
                FinishEventState();
                break;
            case State.Minigame:
                MinigameState();
                break;
            case State.Exit:
                ExitState();
                break;
        }
    }

    IEnumerator WalkingState(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log(":3");
        SetState(State.Event);
    }

    void EventState()
    {
        QuestionScriptable randomEvent = eventManager.GetRandomEvent();

        Debug.Log(randomEvent.question);

        foreach (var answer in randomEvent.answers)
        {
            Debug.Log("Option: " + answer.action);
        }
    }

    void FinishEventState()
    {
        Debug.Log("apply stat changes");
        SetState(State.Walking);
    }

    void MinigameState()
    {
        Debug.Log("startMinigame");
    }

    void ExitState()
    {
        Debug.Log("exit loop");
    }
}
