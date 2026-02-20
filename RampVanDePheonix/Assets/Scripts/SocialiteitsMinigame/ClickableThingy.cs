using UnityEngine;

public class ClickableThingy : MonoBehaviour
{
    private string choice;

    SPSManager spsManager;

    private void Start()
    {
        spsManager = FindAnyObjectByType<SPSManager>();
    }
    public void SteenChoice()
    {
        spsManager.buttons.SetActive(false);
        spsManager.playerHand.SetActive(true);
        spsManager.aiHand.SetActive(true);
        spsManager.playerAnimator.SetInteger("Choice", 0);
        choice = "steen";
        spsManager.ProcessChoice(choice);
    }

    public void PapierChoice()
    {
        spsManager.buttons.SetActive(false);
        spsManager.playerHand.SetActive(true);
        spsManager.aiHand.SetActive(true);
        spsManager.playerAnimator.SetInteger("Choice", 1);
        choice = "papier";
        spsManager.ProcessChoice(choice);
    }

    public void SchaarChoice ()
    {
        spsManager.buttons.SetActive(false);
        spsManager.playerHand.SetActive(true);
        spsManager.aiHand.SetActive(true);
        spsManager.playerAnimator.SetInteger("Choice", 2);
        choice = "schaar";
        spsManager.ProcessChoice(choice);
    }
}
