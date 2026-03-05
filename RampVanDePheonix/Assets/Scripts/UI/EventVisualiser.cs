using UnityEngine;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;

public class EventVisualiser : MonoBehaviour
{
    [Header("Text Elements")]
    [SerializeField] TMP_Text questionInfo;

    [Header("Object References")]
    [SerializeField] GameObject answerButton;
    [SerializeField] GameObject answerField;

    QuestionScriptable current;

    public void FillEventInfo(QuestionScriptable currentEvent)
    {
        current = currentEvent;

        questionInfo.gameObject.SetActive(true);

        questionInfo.text = current.question;
    }

    public void ActionButton()
    {

        questionInfo.gameObject.SetActive(false);

        answerField.SetActive(true);

        for (int i = 0; i < current.answers.Length; i++)
        {
            GameObject currentObject = Instantiate(answerButton,answerField.transform);
            currentObject.GetComponent<AnswerData>().answerText.text = current.answers[i].action;
            currentObject.GetComponent<AnswerData>().answerIndex = i;
        }
        //SET PREVIOUS UI ELEMENTS INACTIVE AND SET NEW ONES ACTIVE
    }

    public QuestionScriptable GetCurrentEvent()
    {
        return current;
    }

}
