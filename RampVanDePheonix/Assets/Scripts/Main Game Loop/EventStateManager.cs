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
    Shop,
    Minigame,
    Exit
}

[Serializable]
public struct Minigame
{
    public string minigameName;
    public Fases allowedPhase;
    public string sceneName;
    public MinigameTutorial tutorialPrefab;
    public Skillset rewardedSkill;
}

public class EventStateManager : MonoBehaviour
{
    public static EventStateManager Instance { get; private set; }

    protected State currentState;

    [SerializeField] protected EventVisualiser eventVisualiser;
    [SerializeField] protected PhaseManager faseManager;
    [SerializeField] protected SceneHider sceneHider;
    [SerializeField] protected MinigameConfig minigameConfig;
    [SerializeField] private RandomEventManager randomEventsManager;
    [SerializeField] private GameObject minigamePauseMenu;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private BackgroundMovement background;

    [Header("LoreDropPrefabs")]
    [SerializeField] GameObject achterhoekLore;
    [SerializeField] GameObject bootLore;
    [SerializeField] GameObject amerikaLore;
    [SerializeField] GameObject lorePosition;

    [SerializeField] private float walkTimeSecsMin = 20;
    [SerializeField] private float walkTimeSecsMax = 30;
    private Coroutine walkingCoroutine;

    protected Dictionary<int, Minigame> minigameTriggers = new();
    public PhaseManager FaseManager { get { return faseManager; } }
    private List<Event> previousEvents = new List<Event>();

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
        eventVisualiser.gameObject.SetActive(false);
    }

    private void SetMinigames()
    {
        minigameTriggers.Clear();

        List<Minigame> allowedMinigames = minigameConfig.minigames.FindAll(m => (m.allowedPhase & faseManager.CurrentFase) != 0);
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
    }

    public virtual void SetState(State newState)
    {
        Debug.Log($"State changed from {currentState} to {newState}");
        currentState = newState;

        switch (currentState)
        {
            case State.Lore:
                Fases currentFase = faseManager.CurrentFase;
                if (currentFase == Fases.Achterhoek) Instantiate(achterhoekLore,lorePosition.transform);
                else if (currentFase == Fases.Pheonix) Instantiate(bootLore, lorePosition.transform);
                else if (currentFase == Fases.Amerika) Instantiate(amerikaLore, lorePosition.transform);

                AudioManager.Instance.SetPhaseMusic(faseManager.CurrentFase);
                AudioManager.Instance.SetPhaseAmbiance(faseManager.CurrentFase);

                // if lore is shown, assume phase got switched or smthn
                if (randomEventsManager != null) randomEventsManager.ClearAllEvents();
                SetMinigames();
                break;
            case State.Walking:
                AudioManager.Instance.SetPhaseMusic(faseManager.CurrentFase);
                AudioManager.Instance.SetPhaseAmbiance(faseManager.CurrentFase);

                if (walkingCoroutine != null) StopCoroutine(walkingCoroutine);
                float walkTimeSecs = Random.Range(walkTimeSecsMin, walkTimeSecsMax);
                walkingCoroutine = StartCoroutine(WalkingState(walkTimeSecs));
                break;
            case State.Shop:
                ShopState(); 
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
        if (randomEventsManager != null) randomEventsManager.StartEventLoop();
        background.StartBackground();
        yield return new WaitForSeconds(waitTime);
        yield return new WaitUntil(() => !Obstacle_Spawn.isBlocking);

        // assume if state changed we no longer wanna do this stuff.
        if (currentState != State.Walking) yield break;

        eventVisualiser.gameObject.SetActive(true);
        SetState(State.Event);
    }


    protected virtual void EventState()
    {
        if (randomEventsManager != null) randomEventsManager.EndEventLoop();
        background.PauseBackground();
        Event randomEvent;
        do
        {
            randomEvent = faseManager.GetRandomEvent();

        } while (previousEvents.Contains(randomEvent));

        eventVisualiser.ShowEvent(randomEvent);
        previousEvents.Add(randomEvent);
    }

    public void ResetPreviousEvents()
    {
        previousEvents.Clear();
    }

    protected virtual void FinishEventState()
    {
        faseManager.AddProgress();
        CharacterListDisplay.Instance.UpdateCharacters();

        SetState(State.Shop);
    }


    [ContextMenu("Lotta progress")]
    public void AddProgress()
    {
        faseManager.AddProgress();
        faseManager.AddProgress();
        faseManager.AddProgress();
        faseManager.AddProgress();
        faseManager.AddProgress();
        faseManager.AddProgress();
        faseManager.AddProgress();
        faseManager.AddProgress();
        faseManager.AddProgress();
        faseManager.AddProgress();
    }

    protected virtual void ShopState()
    {
        bool isPhoenix = faseManager.CurrentFase == Fases.Pheonix;
        bool isBlockedProgress = faseManager.Progress == 0 || faseManager.Progress == 10;
        bool rollShop = Random.Range(0f, 3.33f) < 1f;

        if (!isPhoenix && !isBlockedProgress && rollShop)
        {
            shopPanel.SetActive(true);
            return;
        }

        SetState(State.Minigame);
    }

    protected void MinigameState()
    {
        if (minigameTriggers.TryGetValue(faseManager.Progress, out Minigame selectedMinigame))
        {
            Debug.Log("startMinigame");
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
        if (randomEventsManager != null) randomEventsManager.EndEventLoop();
        Debug.Log("exit loop");
    }


    [ContextMenu("Load Building")]
    public void LoadBuilding() => LoadMinigame(0);

    [ContextMenu("Load Fishing")]
    public void LoadFishing() => LoadMinigame(1);

    [ContextMenu("Load Learning")]
    public void LoadLearning() => LoadMinigame(2);

    [ContextMenu("Load Social")]
    public void LoadSocial() => LoadMinigame(3);
    [ContextMenu("Load Aanpassing")]
    public void LoadAanpassing() => LoadMinigame(4);

    public void LoadMinigame(int index)
    {
        minigamePauseMenu.SetActive(true);
        NextGameManager.Instance.StarMenu(minigameConfig.minigames[index]);
    }

}
