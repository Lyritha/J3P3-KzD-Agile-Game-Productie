using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainGameTutorial : MonoBehaviour
{
    public static MainGameTutorial Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    [SerializeField]
    private TutorialStep[] tutorialSteps;

    private TutorialStep currentStep;

    private int stepIndex = -1;

    private void Start()
    {
        NextStep();
    }

    /// <summary>
    /// use when letting other buttons interact
    /// </summary>
    /// <param name="stepName"></param>
    public void NextStep(string stepName, bool openNextStep = true)
    {
        if (stepName != currentStep.stepName) return;
        NextStep(openNextStep);
    }
    public void NextStep(bool openNextStep = true)
    {
        EndCurrentStep();

        stepIndex++;

        if (stepIndex >= tutorialSteps.Length)
        {
            CompleteTutorial();
            return;
        }

        currentStep = tutorialSteps[stepIndex];
        if (openNextStep)
            StartStep();
    }

    public void StartStep()
    {
        if (currentStep == null) return;
            currentStep.onStepStart?.Invoke();

        foreach (RectTransform ui in currentStep.stepUI)
            ui.gameObject.SetActive(true);

        Time.timeScale = currentStep.pauseTime ? 0 : 1;
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
