using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameFinished : MonoBehaviour
{
    public static MinigameFinished Instance;

    private int score;
    [SerializeField] TMP_Text scoreText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowScore(int score)
    {
        this.score = score;
        scoreText.text = $"SCORE: {score}";
    }


    public void ContinueButton()
    {
        if(EventStateManager.Instance != null)
            EventStateManager.Instance.SetState(State.Walking);

        // Get the current scene this script is part of
        Scene currentScene = gameObject.scene;

        // Show main scene if a SceneHider exists
        SceneHider sceneHider = FindAnyObjectByType<SceneHider>();
        if (sceneHider != null) sceneHider.ShowMainScene();

        // Unload it asynchronously
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
