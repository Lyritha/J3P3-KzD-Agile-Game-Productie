using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum State
{
    Lore,
    Walking,
    Event,
    FinishEvent,
    Minigame,
    Exit
}

[Serializable]
public struct Minigame
{
    public Fases allowedPhase;
    public string sceneName;
    public string minigameName;
}

public class EventStateManager : MonoBehaviour
{
    public static EventStateManager Instance { get; private set; }

    protected State currentState;

    [SerializeField] protected EventVisualiser eventVisualiser;
    [SerializeField] protected PhaseManager faseManager;
    [SerializeField] protected SceneHider sceneHider;
    [SerializeField] protected List<Minigame> minigames;
    [SerializeField] private GameObject minigamePauseMenu;

    [Header("LoreDropPrefabs")]
    [SerializeField] GameObject achterhoekLore;
    [SerializeField] GameObject bootLore;
    [SerializeField] GameObject amerikaLore;
    [SerializeField] GameObject lorePosition;

    protected Dictionary<int, Minigame> minigameTriggers = new();

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    protected void Start()
    {
        minigamePauseMenu.SetActive(false);
        List<Minigame> allowedMinigames = minigames.FindAll(m => (m.allowedPhase & faseManager.CurrentFase) != 0);
        if (allowedMinigames.Count == 0)
        {
            Debug.LogWarning("No minigames available for the current phase.");
            return;
        }

        int maxTriggers = Mathf.Min(2, allowedMinigames.Count); // can't pick more than available
        int attempts = 0;

        while (minigameTriggers.Count < maxTriggers && allowedMinigames.Count > 0)
        {
            attempts++;
            if (attempts > 100) break; // fail-safe to avoid infinite loop

            int minigameIndex = Random.Range(0, allowedMinigames.Count);
            Minigame randomMinigame = allowedMinigames[minigameIndex];

            int triggerPoint = Random.Range(1, 10);
            if (minigameTriggers.ContainsKey(triggerPoint)) continue;

            // Assign minigame and remove it immediately from allowed list
            minigameTriggers.Add(triggerPoint, randomMinigame);
            allowedMinigames.RemoveAt(minigameIndex);
        }

        eventVisualiser.gameObject.SetActive(false);
        SetState(State.Lore);
    }

    public virtual void SetState(State newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case State.Lore:
                Fases currentFase = faseManager.CurrentFase;
                if (currentFase == Fases.Achterhoek) Instantiate(achterhoekLore,lorePosition.transform);
                else if (currentFase == Fases.Pheonix) Instantiate(bootLore, lorePosition.transform);
                else if (currentFase == Fases.Amerika) Instantiate(amerikaLore, lorePosition.transform);
                //instantiate thing
                //start voice
                break;
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

    protected IEnumerator WalkingState(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        eventVisualiser.gameObject.SetActive(true);
        SetState(State.Event);
    }

    protected virtual void EventState()
    {
        Event randomEvent = faseManager.GetRandomEvent();
        eventVisualiser.ShowEvent(randomEvent);
    }

    protected virtual void FinishEventState()
    {
        Debug.Log("apply stat changes");
        faseManager.AddProgress();
        CharacterListDisplay.Instance.UpdateCharacters();

        // try to trigger a minigame, if not possible, go back to walking
        SetState(State.Minigame);
    }

    protected void MinigameState()
    {
        if (minigameTriggers.TryGetValue(faseManager.Progress, out Minigame selectedMinigame))
        {
            Debug.Log("startMinigame");
            // menu openen
            // menu.startMenu(selectedMinigame)
            minigamePauseMenu.SetActive(true);
            NextGameManager.Instance.StarMenu(selectedMinigame);

            return;
        }


        if (faseManager.Progress != 0)
        {
            SetState(State.Walking);
        }
    }

    protected void ExitState()
    {
        Debug.Log("exit loop");
    }

}
