using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text highscoreText;

    private void Start()
    {
        int thisGameScore = PlayerPrefs.GetInt("GameScore", 0);
        int highscore = PlayerPrefs.GetInt("Highscore", 0);

        if (thisGameScore > highscore)
        {
            highscore = thisGameScore;
            PlayerPrefs.SetInt("Highscore", highscore);
        }

        scoreText.text = $"Score: {thisGameScore}";
        highscoreText.text = $"Highscore: {highscore}";
    }
}
