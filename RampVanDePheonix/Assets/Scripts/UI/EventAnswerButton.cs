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

    public void Initialize(string answer, int index, Event currentEvent, EventVisualiser parent)
    {
        selector = CharacterListDisplay.Instance;
        this.parent = parent;
        current = currentEvent;

        answerText.text = answer;
        answerIndex = index;
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
        else
        {
            //tooltip voor errors laten zien
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
        // get reference to personage to make rest more readable (and way more optimized)
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
                }
            }
        }
        // don't use else if here, saver to just do else to allow it to fall back to this
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
                }
            }
        }

        personage.NotifyChanged();
        string result = hasSkills ? current.answers[answerIndex].result : current.answers[answerIndex].resultFailed;
        parent.ShowResult(result);
    }
}
