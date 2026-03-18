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

    void SwapFase(Fases fase)
    {
        CurrentFase = fase; 
        switch (CurrentFase)
        {
            case Fases.Achterhoek:
                FillList(eventsConfig.achterhoekEvents);
                break;
            case Fases.Pheonix:
                FillList(eventsConfig.pheonixEvents);
                break;
            case Fases.Amerika:
                FillList(eventsConfig.amerikaEvents);
                break;
            case Fases.EndScreen:
                SceneManager.LoadScene("EndScreen_Win");
                break;
        }

        display.SetPhase(CurrentFase,10);
        movement.SetFase(CurrentFase);
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
            display.SetPhase(GetNextPhase(CurrentFase), 10);
            SwapFase(GetNextPhase(CurrentFase));
            stateManager.SetState(State.Lore);

            Progress = 0;
        }

        display.IncrementProgress();
        //UPDATE UI
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
