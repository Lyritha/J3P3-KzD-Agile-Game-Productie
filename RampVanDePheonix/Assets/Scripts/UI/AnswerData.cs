using TMPro;
using UnityEngine;

public class AnswerData : MonoBehaviour
{
    public TMP_Text answerText;
    public int answerIndex;
    QuestionScriptable current;

    StateMachine stateMachine;
    CharacterListDisplay selector;
    EventVisualiser visualiser;

    private void Start()
    {
        stateMachine = FindAnyObjectByType<StateMachine>();
        visualiser = FindAnyObjectByType<EventVisualiser>();
        selector = FindAnyObjectByType<CharacterListDisplay>();
    }

    public void AnswerEffect()
    {
        current = visualiser.GetCurrentEvent();
        //check of een personage selected is
        if (selector.SelectedCharacter != null)
        {
            print("selected character is not null");
            //check of benodigde vaardigheden genoeg zijn
            if(CheckSkills()) ChangeSkills();
            //laat uitkomst zien met gebruik van tooltip
            ResetObject();
        }
        else
        {
            //tooltip voor errors laten zien
        }

    }
    bool CheckSkills()
    {
        foreach(var skill in current.answers[answerIndex].skillNeeded)
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
    void ChangeSkills()
    {
        print("entered method to chaneg skills");
        foreach (var change in current.answers[answerIndex].change)
        {
            switch (change.skillToBeChanged)
            {
                case Skillset.Kapitaal:
                    print("changing kapitaal");
                    selector.SelectedCharacter.Personage.baseKapitaal += change.changeAmount;
                    break;
                case Skillset.Bouwkunde:
                    print("changing bouwkunde");
                    selector.SelectedCharacter.Personage.baseBouwkunde += change.changeAmount;
                    break;
                case Skillset.Socialiteit:
                    print("changing socialiteit");
                    selector.SelectedCharacter.Personage.baseSociaal += change.changeAmount;
                    break;
                case Skillset.AanpassingsVermogen:
                    print("changing aanpassingsvermogen");
                    selector.SelectedCharacter.Personage.baseAanpassingsvermogen += change.changeAmount;
                    break;
                case Skillset.Leervermogen:
                    print("changing leervermogen");
                    selector.SelectedCharacter.Personage.baseLeervermogen += change.changeAmount;
                    break;
            }
        }
        selector.SelectedCharacter.Personage.NotifyChanged();
    }

    void ResetObject()
    {
        AnswerData[] answers = FindObjectsByType<AnswerData>(FindObjectsSortMode.None);
        foreach (AnswerData question in answers)
        {
            Destroy(question.gameObject);
        }

        FindAnyObjectByType<EventVisualiser>().gameObject.SetActive(false);

        stateMachine.SetState(State.FinishEvent);
    }
}
