using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainGameTutorial : MonoBehaviour
{
    [SerializeField]
    private TutorialStep[] tutorialSteps;

    private TutorialStep currentStep;

    private int stepIndex = -1;

    private void Start()
    {
        NextStep();
    }

    public void NextStep()
    {
        EndCurrentStep();

        stepIndex++;

        if (stepIndex >= tutorialSteps.Length)
        {
            CompleteTutorial();
            return;
        }

        currentStep = tutorialSteps[stepIndex];
        StartStep(currentStep);
    }

    private void StartStep(TutorialStep step)
    {
        step.onStepStart?.Invoke();
        Time.timeScale = step.pauseTime ? 0 : 1;

        foreach (RectTransform ui in step.stepUI)
            ui.gameObject.SetActive(true);
    }

    private void EndCurrentStep()
    {
        if (currentStep == null) return;
        currentStep.onStepEnd?.Invoke();

        foreach (RectTransform ui in currentStep.stepUI)
            ui.gameObject.SetActive(false);

        Time.timeScale = 1;
    }

    public void CompleteTutorial()
    {
        Debug.Log("Tutorial completed!");
    }
}

[Serializable]
public class TutorialStep
{
    public string stepName;
    public List<RectTransform> stepUI;
    public UnityEvent onStepStart;
    public UnityEvent onStepEnd;
    public bool pauseTime;
}
