using TMPro;
using UnityEngine;

public class MinigameFinished : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowScore(int score)
    {
        scoreText.text = $"SCORE: {score}";
    }

    public void ShowScreen()
    {
        gameObject.SetActive(true);
    }

    public void ContinueButton()
    {
        //write my code twin <3
    }
}
