using UnityEngine;

public class LoreButton : MonoBehaviour
{
    EventStateManager stateManager;
    private void Start()
    {
        stateManager = FindAnyObjectByType<EventStateManager>();
    }
    public void ContinueButton()
    {
        stateManager.SetState(State.Walking);
        Destroy(gameObject.transform.parent.parent.gameObject);
    }
}
