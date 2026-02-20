using UnityEngine;

//SPS staat voor Steen Papier Schaar btw
public class SPSManager : MonoBehaviour
{
    [SerializeField] private GameObject allPrefabs;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] public Animator playerAnimator;
    [SerializeField] public Animator aiAnimator;
    [SerializeField] public GameObject playerHand;
    [SerializeField] public GameObject aiHand;
    [SerializeField] public GameObject buttons;

    private int randomInt;
    private string aiChoice;
    private string gameResult;

    void Start()
    {
        buttons.SetActive(true);
        playerHand.SetActive(false);
        aiHand.SetActive(false);
    }

    public void ProcessChoice(string choice)
    {
        AiChooses();

        if (aiChoice == choice)
        {
            gameResult = "draw";
            print(gameResult);
        }

        else if (aiChoice == "steen" && choice == "papier")
        {
            gameResult = "Player Wins";
            print(gameResult);
        }

        else if (aiChoice == "steen" && choice == "schaar")
        {
            gameResult = "AI Win";
            print(gameResult);
        }

        else if (aiChoice == "papier" && choice == "steen")
        {
            gameResult = "AI Win";
            print(gameResult);
        }

        else if (aiChoice == "papier" && choice == "schaar")
        {
            gameResult = "Player Wins";
            print(gameResult);
        }

        else if (aiChoice == "schaar" && choice == "papier")
        {
            gameResult = "AI Win";
            print(gameResult);
        }

        else if (aiChoice == "schaar" && choice == "steen")
        {
            gameResult = "Player Wins";
            print(gameResult);
        }
    }

    private void AiChooses()
    {
        randomInt = Random.Range(0,3);

        switch (randomInt)
        {
            case 0:
                aiAnimator.SetInteger("Choice", 0);
                aiChoice = "steen";
                break;
            case 1:
                aiAnimator.SetInteger("Choice", 1);
                aiChoice = "papier";
                break;
            case 2:
                aiAnimator.SetInteger("Choice", 2);
                aiChoice = "schaar";
                break;
        }
    }
}
