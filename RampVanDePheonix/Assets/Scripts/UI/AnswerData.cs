using TMPro;
using UnityEngine;

public class AnswerData : MonoBehaviour
{
    public TMP_Text answerText;
    public int answerIndex;
    QuestionScriptable current;

    CharacterListDisplay selector;
    EventVisualiser visualiser;

    private void Start()
    {
        visualiser = FindAnyObjectByType<EventVisualiser>();
        selector = FindAnyObjectByType<CharacterListDisplay>();
    }

    public void AnswerEffect()
    {
        current = visualiser.GetCurrentEvent();
        //check of een personage selected is
        if (selector.SelectedCharacter != null)
        {
            //check of benodigde vaardigheden genoeg zijn
            if (current.answers[answerIndex].skillNeeded.Length > 0)
            {
                if (CheckSkills()) ChangeSkills(false);
                else
                {
                    ChangeSkills(true);
                }
            }
            else
            {
                ChangeSkills(false);
            }            
        }
        else
        {
            //tooltip voor errors laten zien
        }

    }
    bool CheckSkills()
    {
        foreach (var skill in current.answers[answerIndex].skillNeeded)
        {
            switch (skill.skillType)
            {
                case Skillset.Kapitaal:
                    if (selector.SelectedCharacter.Personage.baseKapitaal >= skill.skillAmountNeeded) return true;
                    break;
                case Skillset.Bouwkunde:
                    if (selector.SelectedCharacter.Personage.baseBouwkunde >= skill.skillAmountNeeded) return true;
                    break;
                case Skillset.Socialiteit:
                    if (selector.SelectedCharacter.Personage.baseSociaal >= skill.skillAmountNeeded) return true;
                    break;
                case Skillset.AanpassingsVermogen:
                    if (selector.SelectedCharacter.Personage.baseAanpassingsvermogen >= skill.skillAmountNeeded) return true;
                    break;
                case Skillset.Leervermogen:
                    if (selector.SelectedCharacter.Personage.baseLeervermogen >= skill.skillAmountNeeded) return true;
                    break;
            }
        }
        return false;
    }
    void ChangeSkills(bool failed)
    {
        if (!failed)
        {
            foreach (var change in current.answers[answerIndex].change)
            {
                switch (change.skillToBeChanged)
                {
                    case Skillset.Kapitaal:
                        selector.SelectedCharacter.Personage.baseKapitaal += change.changeAmount;
                        break;
                    case Skillset.Bouwkunde:
                        selector.SelectedCharacter.Personage.baseBouwkunde += change.changeAmount;
                        break;
                    case Skillset.Socialiteit:
                        selector.SelectedCharacter.Personage.baseSociaal += change.changeAmount;
                        break;
                    case Skillset.AanpassingsVermogen:
                        selector.SelectedCharacter.Personage.baseAanpassingsvermogen += change.changeAmount;
                        break;
                    case Skillset.Leervermogen:
                        selector.SelectedCharacter.Personage.baseLeervermogen += change.changeAmount;
                        break;
                }
            }
        }
        else if (failed)
        {
            foreach (var change in current.answers[answerIndex].changeFailed)
            {
                switch (change.skillToBeChanged)
                {
                    case Skillset.Kapitaal:
                        selector.SelectedCharacter.Personage.baseKapitaal += change.changeAmount;
                        break;
                    case Skillset.Bouwkunde:
                        selector.SelectedCharacter.Personage.baseBouwkunde += change.changeAmount;
                        break;
                    case Skillset.Socialiteit:
                        selector.SelectedCharacter.Personage.baseSociaal += change.changeAmount;
                        break;
                    case Skillset.AanpassingsVermogen:
                        selector.SelectedCharacter.Personage.baseAanpassingsvermogen += change.changeAmount;
                        break;
                    case Skillset.Leervermogen:
                        selector.SelectedCharacter.Personage.baseLeervermogen += change.changeAmount;
                        break;
                }
            }
        }
        selector.SelectedCharacter.Personage.NotifyChanged();
        if (failed) ResetObject(current.answers[answerIndex].resultFailed);
        else if (!failed) ResetObject(current.answers[answerIndex].result); 
    }

    void ResetObject(string uitkomst)
    {
        AnswerData[] answers = FindObjectsByType<AnswerData>(FindObjectsSortMode.None);
        foreach (AnswerData question in answers)
        {
            Destroy(question.gameObject);
        }

        GameObject currentObject = FindAnyObjectByType<UitslagText>().gameObject;
        currentObject.SetActive(true);
        currentObject.GetComponent<UitslagText>().ShowText(uitkomst);
    }
}
