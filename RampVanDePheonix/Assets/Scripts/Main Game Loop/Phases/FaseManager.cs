using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public enum Fases
{
    Achterhoek,
    Pheonix,
    Amerika,
    EndScreen
}
public class FaseManager : MonoBehaviour
{
    [Header("Pre-Filled Lists")]
    public List<QuestionScriptable> achterhoekEvents = new List<QuestionScriptable>();
    public List<QuestionScriptable> pheonixEvents = new List<QuestionScriptable>();
    public List<QuestionScriptable> amerikaEvents = new List<QuestionScriptable>();


    [Header("Current Events (dont touch)")]
    public List<QuestionScriptable> currentEvents = new List<QuestionScriptable>();

    PhaseDisplayer display;
    ParallaxSwapper swapper;
    Fases currentFase;
    int progress = 0;
    int maxProgress = 10;
    private void Start()
    {
        display = FindAnyObjectByType<PhaseDisplayer>();
        swapper = FindAnyObjectByType<ParallaxSwapper>();  
        SwapFase(Fases.Achterhoek); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AddProgress();
            print("progress =  " + progress);
        }
    }

    void SwapFase(Fases fase)
    {
        currentFase = fase; 
        switch (currentFase)
        {
            case Fases.Achterhoek:
                FillList(achterhoekEvents);
                break;
            case Fases.Pheonix:
                FillList(pheonixEvents);
                break;
            case Fases.Amerika:
                FillList(amerikaEvents);
                break;
            case Fases.EndScreen:
                SceneManager.LoadScene("EndScreen_Win");
                break;
        }

        display.SetPhase(currentFase,10);
        swapper.SetGrounds(currentFase);
    }

    void FillList(List<QuestionScriptable> incomingEvents)
    {
        currentEvents.Clear();
        foreach(QuestionScriptable p in incomingEvents)
        {
            currentEvents.Add(p);
        }
    }

    public void AddProgress()
    {
        progress++;

        if (progress >= maxProgress)
        {
            display.SetPhase(GetNextPhase(currentFase), 10);
            SwapFase(GetNextPhase(currentFase));

            progress = 0;
        }

        display.IncrementProgress();
        //UPDATE UI
    }

    Fases GetNextPhase(Fases currentPhase)
    {
        if (currentPhase == Fases.Achterhoek) return Fases.Pheonix;
        if(currentPhase == Fases.Pheonix) return Fases.Amerika;
        if (currentFase == Fases.Amerika) return Fases.EndScreen;
        return Fases.Achterhoek;
    }

}
