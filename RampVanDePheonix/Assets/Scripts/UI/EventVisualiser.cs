using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class EventVisualiser : MonoBehaviour
{
    [SerializeField] Sprite Icon_Build;
    [SerializeField] Sprite Icon_Learn;
    [SerializeField] Sprite Icon_Adaptability;
    [SerializeField] Sprite Icon_Social;
    [SerializeField] Sprite Icon_Capital;



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
            List<IconValue> IconValuePairs = AddSkillIcon(currentObject, current.answers[i], current);
            currentObject.Initialize(current.answers[i].action, i, current, this, IconValuePairs.ToArray());
            currentButtons.Add(currentObject);
            //add here skill requirements >>>>>>>>>>>>
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
    private List<IconValue> AddSkillIcon(EventAnswerButton thisButton, Answer currentAnwer, Event eventthing)
    {
        List<IconValue> pairs = new List<IconValue>();
        for (int i = 0; i < currentAnwer.skillNeeded.Length; i++)
        {
            //switch to place in the thing
            Sprite chosenSprite = null;
            int skillAmountNeeded = currentAnwer.skillNeeded[i].skillAmountNeeded;
            switch (currentAnwer.skillNeeded[i].skillType)
            {
                case Skillset.Socialiteit:
                    chosenSprite = Icon_Social;
                    break;
                case Skillset.Kapitaal:
                    chosenSprite = Icon_Capital;
                    break;
                case Skillset.Bouwkunde:
                    chosenSprite = Icon_Learn;
                    break;
                case Skillset.AanpassingsVermogen:
                    chosenSprite = Icon_Adaptability;
                    break;
                case Skillset.Leervermogen:
                    chosenSprite = Icon_Learn;
                    break;
                default:
                    chosenSprite = Icon_Capital;
                    break;
            }

            //create the new value and set the sprite and value needed
            IconValue newvalue = new IconValue();
            newvalue.amountNeeded = skillAmountNeeded;
            newvalue.iconSprite = chosenSprite;
            pairs.Add(newvalue);
        }
        return pairs;
    }
    public struct IconValue
    {
        public Sprite iconSprite;
        public int amountNeeded;
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
