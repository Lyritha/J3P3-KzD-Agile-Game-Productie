using NUnit.Framework;
using TMPro;
using UnityEngine;

public class EventAnswerButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text answerText;
    [SerializeField]
    private int answerIndex;

    private Event current;
    private EventVisualiser parent;
    CharacterListDisplay selector;

    //adding the 
    [SerializeField]
    Sprite iconSprite = null;
    [SerializeField]
    int iconValue;

    public void Initialize(string answer, int index, Event currentEvent, EventVisualiser parent, EventVisualiser.IconValue[] iconValuePair)
    {
        selector = CharacterListDisplay.Instance;
        this.parent = parent;
        current = currentEvent;

        answerText.text = answer;
        answerIndex = index;
        if (iconValuePair.Length > 0)
        {
            foreach (EventVisualiser.IconValue iconValuePairs in iconValuePair)
            {
                iconSprite = iconValuePairs.iconSprite;
                iconValue = iconValuePairs.amountNeeded;
            }
        }
    }

    public void TriggerAnswer()
    {
        //check of een personage selected is
        if (selector.SelectedCharacter != null)
        {
            //check of benodigde vaardigheden genoeg zijn
            if (current.answers[answerIndex].skillNeeded.Length > 0)
            {
                ChangeSkills(CheckSkills());
            }
            else
            {
                ChangeSkills(true);
            }
        }

    }
    bool CheckSkills()
    {
        // get reference to personage to make rest more readable (and way more optimized)
        Personage personage = selector.SelectedCharacter.Personage;

        foreach (var skill in current.answers[answerIndex].skillNeeded)
        {
            switch (skill.skillType)
            {
                case Skillset.Kapitaal:
                    if (personage.baseKapitaal >= skill.skillAmountNeeded) return true;
                    break;
                case Skillset.Bouwkunde:
                    if (personage.baseBouwkunde >= skill.skillAmountNeeded) return true;
                    break;
                case Skillset.Socialiteit:
                    if (personage.baseSociaal >= skill.skillAmountNeeded) return true;
                    break;
                case Skillset.AanpassingsVermogen:
                    if (personage.baseAanpassingsvermogen >= skill.skillAmountNeeded) return true;
                    break;
                case Skillset.Leervermogen:
                    if (personage.baseLeervermogen >= skill.skillAmountNeeded) return true;
                    break;
            }
        }
        return false;
    }
    void ChangeSkills(bool hasSkills)
    {
        Personage personage = selector.SelectedCharacter.Personage;
        


        if (hasSkills)
        {
            foreach (var change in current.answers[answerIndex].change)
            {
                switch (change.skillToBeChanged)
                {
                    case Skillset.Kapitaal:
                        personage.baseKapitaal += change.changeAmount;
                        break;
                    case Skillset.Bouwkunde:
                        personage.baseBouwkunde += change.changeAmount;
                        break;
                    case Skillset.Socialiteit:
                        personage.baseSociaal += change.changeAmount;
                        break;
                    case Skillset.AanpassingsVermogen:
                        personage.baseAanpassingsvermogen += change.changeAmount;
                        break;
                    case Skillset.Leervermogen:
                        personage.baseLeervermogen += change.changeAmount;
                        break;
                    case Skillset.Death:
                        selector.SelectedCharacter.Die("ebola");
                        break;
                }
            }
        }
        else
        {
            foreach (var change in current.answers[answerIndex].changeFailed)
            {
                switch (change.skillToBeChanged)
                {
                    case Skillset.Kapitaal:
                        personage.baseKapitaal += change.changeAmount;
                        break;
                    case Skillset.Bouwkunde:
                        personage.baseBouwkunde += change.changeAmount;
                        break;
                    case Skillset.Socialiteit:
                        personage.baseSociaal += change.changeAmount;
                        break;
                    case Skillset.AanpassingsVermogen:
                        personage.baseAanpassingsvermogen += change.changeAmount;
                        break;
                    case Skillset.Leervermogen:
                        personage.baseLeervermogen += change.changeAmount;
                        break;
                    case Skillset.Death:
                        selector.SelectedCharacter.Die("ebola");
                        break;
                }
            }
        }
        CheckForNegatives(personage);
        personage.NotifyChanged();
        string result = hasSkills ? current.answers[answerIndex].result : current.answers[answerIndex].resultFailed;
        parent.ShowResult(result);
    }

    void CheckForNegatives(Personage personage)
    {
        if(personage.baseKapitaal < 0) personage.baseKapitaal = 0;
        if(personage.baseBouwkunde < 0) personage.baseBouwkunde = 0;
        if(personage.baseSociaal < 0) personage.baseSociaal = 0;
        if(personage.baseAanpassingsvermogen < 0) personage.baseAanpassingsvermogen = 0;
        if(personage.baseLeervermogen < 0) personage.baseLeervermogen = 0;
    }
}
