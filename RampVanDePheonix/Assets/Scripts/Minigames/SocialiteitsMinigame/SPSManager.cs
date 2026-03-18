using System.Threading.Tasks;
using TMPro;
using UnityEngine;

//SPS staat voor Steen Papier Schaar btw
public class SPSManager : MonoBehaviour
{
    [SerializeField] private GameObject allPrefabs;
    [SerializeField] private TMP_Text winLoseText;
    [SerializeField] private TMP_Text scoreText;

    [SerializeField] public Animator playerAnimator;
    [SerializeField] public Animator aiAnimator;

    [SerializeField] public GameObject playerHand;
    [SerializeField] public GameObject aiHand;
    [SerializeField] public GameObject buttons;

    [SerializeField] private int textTimer;
    private int randomInt;
    private string aiChoice;
    private string gameResult;
    public int score;

    void Start()
    {
        score = 0;
        ResetGame();
    }

    void ResetGame()
    {
        winLoseText.gameObject.SetActive(false);
        buttons.SetActive(true);
        playerHand.SetActive(false);
        aiHand.SetActive(false);
    }

    public void ProcessChoice(string choice)
    {
        AiChooses();

        if (aiChoice == choice)
        {
            gameResult = "Gelijkspel.";
            winLoseText.text = gameResult;
        }

        else if (aiChoice == "steen" && choice == "papier")
        {
            score++;
            gameResult = "Jij Wint!";
            winLoseText.text = gameResult;
        }

        else if (aiChoice == "steen" && choice == "schaar")
        {
            gameResult = "Tegenstander Wint...";
            winLoseText.text = gameResult;
        }

        else if (aiChoice == "papier" && choice == "steen")
        {
            gameResult = "Tegenstander Wint...";
            winLoseText.text = gameResult;
        }

        else if (aiChoice == "papier" && choice == "schaar")
        {
            score++;
            gameResult = "Jij Wint!";
            winLoseText.text = gameResult;
        }

        else if (aiChoice == "schaar" && choice == "papier")
        {
            gameResult = "Tegenstander Wint...";
            winLoseText.text = gameResult;
        }

        else if (aiChoice == "schaar" && choice == "steen")
        {
            score++;
            gameResult = "Jij Wint!";
            winLoseText.text = gameResult;
        }

        Invoke(nameof(ShowResults), textTimer);
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

    private void ShowResults()
    {
        MinigameFinished.Instance.ShowScore(score);
        scoreText.text = "Score: " + score;
        winLoseText.gameObject.SetActive(true);
        Invoke(nameof(ResetGame), 1);
    }
}
