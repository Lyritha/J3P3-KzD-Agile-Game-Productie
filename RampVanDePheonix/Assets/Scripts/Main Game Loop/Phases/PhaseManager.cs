using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    [SerializeField]
    private PhaseDisplayer display;
    [SerializeField]
    private EventStateManager stateManager;
    [SerializeField] 
    private EventsConfig eventsConfig;


    public Fases CurrentFase { get; private set; }
    public int Progress { get; private set; } = 0;
    private List<Event> currentEvents = new List<Event>();


    BackgroundMovement movement;
    int maxProgress = 10;

    private void Start()
    {
        movement = FindAnyObjectByType<BackgroundMovement>();  
        SwapFase(Fases.Achterhoek); 
    }

    public void SwapFase(Fases fase)
    {
        Progress = 0;
        switch (fase)
        {
            case Fases.Achterhoek:
                FillList(eventsConfig.achterhoekEvents);
                break;
            case Fases.Pheonix:
                FillList(eventsConfig.pheonixEvents);
                break;
            case Fases.Amerika:
                FillList(eventsConfig.amerikaEvents);
                if (fase != CurrentFase) CharacterListDisplay.Instance.KillAllUnsafe();
                
                break;
            case Fases.EndScreen:

                stateManager.SetState(State.Exit);
                FindFirstObjectByType<SceneHider>().HideMainScene();
                SceneManager.LoadScene("EndScreen_Win", LoadSceneMode.Additive);
                break;
        }

        CurrentFase = fase; 
        display.SetPhase(CurrentFase,10);
        movement.SetFase(CurrentFase);
        stateManager.SetState(State.Lore);
    }


    [ContextMenu("finish game")]
    public void FinishGame()
    {
        SwapFase(Fases.EndScreen);
    }


    void FillList(List<Event> incomingEvents)
    {
        currentEvents.Clear();
        foreach(Event p in incomingEvents)
        {
            currentEvents.Add(p);
        }
    }

    public void AddProgress()
    {
        Progress++;

        if (Progress >= maxProgress)
        {
            Fases nextFase = GetNextPhase(CurrentFase);
            display.SetPhase(nextFase, 10);
            SwapFase(nextFase);
        }

        display.IncrementProgress();
    }

    Fases GetNextPhase(Fases currentPhase)
    {
        if (currentPhase == Fases.Achterhoek) return Fases.Pheonix;
        if(currentPhase == Fases.Pheonix) return Fases.Amerika;
        if (CurrentFase == Fases.Amerika) return Fases.EndScreen;
        return Fases.Achterhoek;
    }

    public Event GetRandomEvent()
    {
        int randomIndex = Random.Range(0, currentEvents.Count);
        return currentEvents[randomIndex];
    }
}
