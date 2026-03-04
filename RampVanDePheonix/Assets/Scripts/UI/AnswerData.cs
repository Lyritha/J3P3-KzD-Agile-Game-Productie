using UnityEngine;
using TMPro;

public class AnswerData : MonoBehaviour
{
    public TMP_Text answerText;
    public int answerIndex;

    StateMachine stateMachine;

    public void AnswerEffect()
    {
        AnswerData[] answers = FindObjectsByType<AnswerData>(FindObjectsSortMode.None);
        foreach(AnswerData question in answers)
        {
            Destroy(question.gameObject);
        }

        FindAnyObjectByType<EventVisualiser>().gameObject.SetActive(false);

        //stateMachine.SetState(State.FinishEvent);

    }
}
