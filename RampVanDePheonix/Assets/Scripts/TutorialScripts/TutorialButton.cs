using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    [SerializeField]
    private string eventName;

    public void TriggerButtonNext()
    {
        MainGameTutorial.Instance.NextStep(eventName);
    }

    public void TriggerButtonEndStep()
    {
        MainGameTutorial.Instance.NextStep(eventName, false);
    }
}
