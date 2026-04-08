using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class EventVisualiser : MonoBehaviour
{
    [SerializeField] Sprite Icon_Build;
    [SerializeField] Sprite Icon_Learn;
    [SerializeField] Sprite Icon_Adaptability;
    [SerializeField] Sprite Icon_Social;
    [SerializeField] Sprite Icon_Capital;
    [SerializeField] Sprite Icon_Question;
    [SerializeField] Sprite Icon_Food;

    [Header("Vraag")]
    [SerializeField] GameObject questionParent;
    [SerializeField] TMP_Text questionInfo;

    [Header("Antwoorden")]
    [SerializeField] GameObject answerParent;
    private List<EventAnswerButton> currentButtons = new();

    [Header("Uitkomst")]
    [SerializeField] GameObject resultParent;
    [SerializeField] TMP_Text resultText;

    [Header("CharacterIcon")]
    [SerializeField] Image characterImage;
    [SerializeField] TMP_Text characterName;


    [Header("Object References")]
    [SerializeField] EventAnswerButton answerButtonPrefab;

    private Event current;

    private void Update()
    {
        //wait for a character to be selected
        CheckCharacterSelect();
    }

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
            List<IconValue> IconValuePairs = AddSkillIcon(currentObject, current.answers[i]);
            currentObject.Initialize(current.answers[i].action, i, current, this, IconValuePairs.ToArray());
            currentButtons.Add(currentObject);
        }
    }
    public void ShowResult(string result)
    {
        resultText.text = result;
        ToggleUI(2);
    }

    public void Hide(bool goToNextEvent = true)
    {
        if (TryGetComponent(out PopupAnim anim))
        {
            anim.CloseAnim();
        }
        else
        {
            gameObject.SetActive(false);
        }


        if (goToNextEvent) EventStateManager.Instance.SetState(State.FinishEvent);
    }

    /// <summary>
    /// method that creates the pair (icon and value) needed for the answer its reading
    /// </summary>
    private List<IconValue> AddSkillIcon(EventAnswerButton thisButton, Answer currentAnwer)
    {
        List<IconValue> pairs = new List<IconValue>();

        //switch for determening which icon (image) it needs according to skillNeeded(enum) from the current answer
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
                    chosenSprite = Icon_Build;
                    break;
                case Skillset.AanpassingsVermogen:
                    chosenSprite = Icon_Adaptability;
                    break;
                case Skillset.Leervermogen:
                    chosenSprite = Icon_Learn;
                    break;
                case Skillset.FoodStorage:
                    chosenSprite = Icon_Food;
                    break;
                default:
                    chosenSprite = Icon_Question;
                    break;
            }

            //create the new value and set the sprite and value needed
            IconValue newvalue = new IconValue();
            newvalue.amountNeeded = skillAmountNeeded;
            newvalue.iconSprite = chosenSprite;

            //adds the pair who is just created to the list
            pairs.Add(newvalue);
        }
        //returns the whole list of pairs 
        return pairs;
    }

    //struct for determening which items will be needed for an answer icon
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

    void PlaceSelectedCharacterOnCanvas(Character selectedCharacter)
    {
        if (selectedCharacter != null)
        {
            characterImage.sprite = selectedCharacter.Personage.portrait;
            characterName.text = selectedCharacter.Personage.characterName;
        }
        else
        {
            characterImage.sprite = Icon_Question;
            characterName.text = "Kies een karakter";
        }

    }
    void CheckCharacterSelect()
    {
        //checks if a character has been selected
        if (CharacterListDisplay.Instance.SelectedCharacter != null)
        {
            PlaceSelectedCharacterOnCanvas(CharacterListDisplay.Instance.SelectedCharacter);
        }
        else
        {
            PlaceSelectedCharacterOnCanvas(null);
        }
    }
}

