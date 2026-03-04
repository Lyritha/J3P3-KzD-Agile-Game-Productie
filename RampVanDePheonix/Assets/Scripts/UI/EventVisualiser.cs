using UnityEngine;
using TMPro;

public class EventVisualiser : MonoBehaviour
{
    [Header("Text Elements")]
    [SerializeField] TMP_Text questionInfo;

    [Header("Object References")]
    [SerializeField] GameObject answerButton;
    [SerializeField] GameObject answerField;

    [SerializeField] QuestionScriptable current; //GET RID OF SERIALIEZFIELD WHEN DONE
    public bool placeholderMode;

    private void Start()
    {
        if(placeholderMode) FillEventInfo(current); // PLACEHOLDER
    }

    public void FillEventInfo(QuestionScriptable currentEvent)
    {
        currentEvent = current;

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
}
