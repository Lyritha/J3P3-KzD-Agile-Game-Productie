using UnityEngine;
using TMPro;

public class AnswerData : MonoBehaviour
{
    public TMP_Text answerText;
    public int answerIndex;

    public void AnswerEffect()
    {
        Destroy(FindAnyObjectByType<EventVisualiser>().gameObject);
    }
}
