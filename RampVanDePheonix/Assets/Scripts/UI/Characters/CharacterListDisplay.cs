using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterListDisplay : MonoBehaviour
{
    public static CharacterListDisplay Instance { get; private set; }

    [SerializeField]
    protected Character characterPrefab;
    [SerializeField]
    protected RectTransform characterParent;
    [SerializeField]
    protected Capital capitalDisplay;

    public Character SelectedCharacter;
    public List<Character> Characters { get; private set; } = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void DeselectCharacter()
    {
        for (int i = 0; i < Characters.Count; i++)
        {
            Character otherCharacter = Characters[i];
            otherCharacter.SetSelected(false);

            NextGameManager manager = FindFirstObjectByType<NextGameManager>();
            if (manager != null) manager.UpdateSelected(i, false);
        }

        SelectedCharacter = null;
    }

    // Handle setting the selected character
    public void SetSelectedCharacter(Character character)
    {
        for(int i = 0; i < Characters.Count; i++)
        {
            Character otherCharacter = Characters[i];
            bool isSelected = character != null && otherCharacter.Personage == character.Personage;
            otherCharacter.SetSelected(isSelected);

            NextGameManager manager = FindFirstObjectByType<NextGameManager>();
            if (manager != null) manager.UpdateSelected(i, isSelected);
        }

        SelectedCharacter = character;
    }
    public bool TryGetSelectedCharacter(out Character character)
    {
        character = SelectedCharacter;
        return character != null;
    }


    // handle adding characters to the list, uncluding UI
    public void AddCharacters(List<Personage> characters) => AddCharacters(characters.ToArray());
    public virtual void AddCharacters(Personage[] personages)
    {
        ClearCharacters();

        foreach (Personage personage in personages)
            AddCharacter(personage);

        capitalDisplay.AddCapitalItems(personages);
    }
    protected void AddCharacter(Personage personage)
    {
        Character character = Instantiate(characterPrefab, characterParent);
        Characters.Add(character);
        character.Initialize(personage);
    }



    /// <summary>
    /// trigger update on all characters, can be called every day or something like that to make characters consume food and stuff
    /// </summary>
    public void UpdateCharacters()
    {
        foreach (Character character in Characters)
            if (character.IsAlive) character.UpdateCharacterState();
    }

    /// <summary>
    /// call when a character died, checks if all characters are dead and if so, shows game over screen or something similar
    /// </summary>
    public void ReportCharacterDied()
    {
        foreach (Character character in Characters)
            if (character.IsAlive) return;

        // show game over screen or something similar here
        Debug.Log("All characters have died. Game Over.");
        SceneManager.LoadScene("EndScreen_Lose");
    }


    protected void ClearCharacters()
    {
        foreach (Transform child in characterParent)
            Destroy(child.gameObject);

        Characters.Clear();
    }
}
