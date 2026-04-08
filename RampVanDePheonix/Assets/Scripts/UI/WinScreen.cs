using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField]
    private CalculateEndScore endScore;

    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text highscoreText;

    [SerializeField]
    private TMP_Text buttonText;
    [SerializeField]
    private Button button;

    [SerializeField]
    private RectTransform loreRect;
    [SerializeField]
    private RectTransform scoreRect;

    private int screen = 0;
    private int highscore = 0;

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        highscoreText.text = $"Highscore: {highscore}";
        buttonText.text = $"Score";
    }

    public void UpdateScore(int score)
    {
        if (score > highscore)
        {
            highscore = score;
            highscoreText.text = $"Highscore: {highscore}";
            PlayerPrefs.SetInt("Highscore", highscore);
        }

        scoreText.text = $"Score: {score}";
    }

    public void StartShowStats()
    {
        switch (screen)
        {
            case 0:
                endScore.ShowStats();
                ToggleButton(false);
                buttonText.text = $"Verder";
                break;

            case 1:
                loreRect.gameObject.SetActive(true);
                scoreRect.gameObject.SetActive(false);
                buttonText.text = $"Terug naar hoofdmenu";
                break;

            case 2:
                SceneManager.LoadScene("Main Menu");
                break;
        }

        screen++;
    }

    public void ToggleButton(bool active) => button.interactable = active;
}
