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
    [SerializeField] EventVisualiser eventVisualiser;
    [SerializeField] GameObject eventQuestion;

    FaseManager faseManager;

    private void Start()
    {
        faseManager = FindAnyObjectByType<FaseManager>();
        eventQuestion.SetActive(false);
        SetState(State.Walking);
    }

    public void SetState(State newState)
    {
        state = newState;

        switch (state)
        {
            case State.Walking:
                StartCoroutine(WalkingState(2));
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
        eventQuestion.SetActive(true);
        SetState(State.Event);
    }

    void EventState()
    {
        QuestionScriptable randomEvent = eventManager.GetRandomEvent();
        eventVisualiser.FillEventInfo(randomEvent);
    }

    void FinishEventState()
    {
        Debug.Log("apply stat changes");
        faseManager.AddProgress();
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
