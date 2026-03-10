using UnityEngine;
using TMPro;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UI;

public class EventVisualiser : MonoBehaviour
{
    [Header("Vraag")]
    [SerializeField] GameObject questionParent;
    [SerializeField] TMP_Text questionInfo;

    [Header("Antwoorden")]
    [SerializeField] GameObject answerParent;
    private List<EventAnswerButton> currentButtons = new();

    [Header("Uitkomst")]
    [SerializeField] GameObject resultParent;
    [SerializeField] TMP_Text resultText;


    [Header("Object References")]
    [SerializeField] EventAnswerButton answerButtonPrefab;


    private Event current;

    public void ShowEvent(Event currentEvent)
    {
        current = currentEvent;

        questionInfo.text = current.question;
        ToggleUI(0);
    }

    // builds the button ui
    public void ShowActions()
    {
        ToggleUI(1);

        for (int i = 0; i < current.answers.Length; i++)
        {
            EventAnswerButton currentObject = Instantiate(answerButtonPrefab, answerParent.transform);
            currentObject.Initialize(current.answers[i].action, i, current, this);
            currentButtons.Add(currentObject);
        }
    }
    public void ShowResult(string result)
    {
        resultText.text = result;
        ToggleUI(2);
        Invoke(nameof(HideAfterDelay), 2f);
    }

    private void HideAfterDelay()
    {
        gameObject.SetActive(false);
        EventStateManager.Instance.SetState(State.FinishEvent);
    }


    // Method to clear existing buttons
    private void ClearButtons()
    {
        foreach (EventAnswerButton button in currentButtons)
            Destroy(button.gameObject);

        currentButtons.Clear();
    }

    public void ToggleUI(int selector)
    {

        switch (selector)
        {
            case 0:
                questionParent.SetActive(true);
                answerParent.SetActive(false);
                resultParent.SetActive(false);
                break;

            case 1:
                questionParent.SetActive(false);
                answerParent.SetActive(true);
                resultParent.SetActive(false);
                break;

            case 2:
                questionParent.SetActive(false);
                ClearButtons();
                answerParent.SetActive(false);
                resultParent.SetActive(true);
                break;
        }
    }
}
