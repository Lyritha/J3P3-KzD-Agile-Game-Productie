using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameFinished : MonoBehaviour
{
    public static MinigameFinished Instance;

    public Minigame minigame;

    private int score;
    [SerializeField] TMP_Text scoreText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;

    }

    public void Init(Minigame minigame)
    {
        this.minigame = minigame;
        gameObject.SetActive(false);
    }

    public void ShowScore(int score)
    {
        this.score = score;
        scoreText.text = $"SCORE: {score}";
    }


    public void ContinueButton()
    {
        RewardSkill();

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

    public void RewardSkill()
    {
        Character character = CharacterListDisplay.Instance.SelectedCharacter;
        if (character == null ) return;

        Personage personage = character.Personage;

        switch (minigame.rewardedSkill)
        {
            case Skillset.Kapitaal:
                personage.baseKapitaal += score;
                break;

            case Skillset.Socialiteit:
                personage.baseSociaal += score;
                break;

            case Skillset.Bouwkunde:
                personage.baseBouwkunde += score;
                break;

            case Skillset.AanpassingsVermogen:
                personage.baseAanpassingsvermogen += score;
                break;

            case Skillset.Leervermogen:
                personage.baseLeervermogen += score;
                break;

            default:
                personage.baseKapitaal += score;
                break;
        }

        character.Personage.NotifyChanged();
    }
}
