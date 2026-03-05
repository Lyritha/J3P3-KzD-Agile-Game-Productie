using UnityEngine;
using TMPro;
public class UitslagText : MonoBehaviour
{
    [SerializeField] TMP_Text uitslagText;
    StateMachine stateMachine;
    private void Start()
    {
        stateMachine = FindAnyObjectByType<StateMachine>();
    }

    public void ShowText(string text)
    {
        uitslagText.gameObject.SetActive(true);
        uitslagText.text = text;
        Invoke("ResetText", 3);
    }

    void ResetText()
    {
        stateMachine.SetState(State.FinishEvent);
        FindAnyObjectByType<EventVisualiser>().gameObject.SetActive(false);
        uitslagText.gameObject.SetActive(false);
    }
}
