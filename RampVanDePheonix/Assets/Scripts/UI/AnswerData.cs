using UnityEngine;
using TMPro;

public class AnswerData : MonoBehaviour
{
    public TMP_Text answerText;
    public int answerIndex;

    public void AnswerEffect()
    {
        AnswerData[] answers = FindObjectsByType<AnswerData>(FindObjectsSortMode.None);
        foreach(AnswerData question in answers)
        {
            Destroy(question.gameObject);
        }
    }
}
