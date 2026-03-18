using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextGameManager : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] Character characterPrefab;
    [SerializeField] SceneHider sceneHider;

    [SerializeField] TMP_Text nextGameText;
    [SerializeField] RectTransform tutorialParent;

    private bool canPlay = false;
    private Minigame chosenMinigame;

    public static NextGameManager Instance { get; private set; }
    public List<Character> Characters { get; private set; } = new();

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StarMenu(Minigame selectedMinigame)
    {
        chosenMinigame = selectedMinigame;
        nextGameText.text = $"Volgende Minigame: {chosenMinigame.minigameName}";

        foreach (Character character in Characters)
        {
            Destroy(character.gameObject);
        }

        Characters.Clear();

        CharacterListDisplay display = CharacterListDisplay.Instance;
        display.DeselectCharacter();


        foreach (Character character in display.Characters)
        {
            Character newCharacter = Instantiate(characterPrefab, spawnPoint);
            Characters.Add(newCharacter);
            newCharacter.Initialize(character.Personage, character.IsAlive, character.IsOnLifeBoat);
        }
    }

    public void UpdateSelected(int index, bool isSelected)
    {
        if (index >= Characters.Count) return;
        Characters[index].SetSelected(isSelected);
    }

    private void Update()
    {
        canPlay = CharacterListDisplay.Instance.SelectedCharacter != null;
    }


    public void StartGame()
    {
        if (!canPlay) return;
        gameObject.SetActive(false);
        sceneHider.HideMainScene();

        StartCoroutine(LoadMinigame(chosenMinigame));
    }

    IEnumerator LoadMinigame(Minigame chosenMinigame)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(chosenMinigame.sceneName, LoadSceneMode.Additive);

        yield return op; 

        MinigameFinished.Instance.Init(chosenMinigame);
    }

    public void StartTutorial()
    {
        Instantiate(chosenMinigame.tutorialPrefab, tutorialParent);
    }
}
