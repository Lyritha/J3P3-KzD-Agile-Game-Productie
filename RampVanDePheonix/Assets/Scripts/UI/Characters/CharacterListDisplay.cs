using MyBox;
using System.Collections.Generic;
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
            if (character.IsSelectable) character.UpdateCharacterState();
    }

    /// <summary>
    /// call when a character died, checks if all characters are dead and if so, shows game over screen or something similar
    /// </summary>
    public void ReportCharacterDied()
    {
        bool isSomeoneAlive = false;
        for (int i = 0; i < Characters.Count; i++)
        {
            Character character = Characters[i];

            if (character.IsAlive) isSomeoneAlive = true;
            else capitalDisplay.ClearCapitalItem(character.Personage);
        }

        if (isSomeoneAlive)
        {
            ReportCharacterStateChanged();
            return;
        }

        // show game over screen or something similar here
        Debug.Log("All characters have died. Game Over.");
        SceneManager.LoadScene("EndScreen_Lose");
    }

    public void ReportCharacterStateChanged()
    {
        PhaseManager faseManager = FindFirstObjectByType<PhaseManager>();
        if (faseManager.CurrentFase == Fases.Pheonix && Characters.TrueForAll(c => !c.IsSelectable))
            faseManager.SwapFase(Fases.Amerika);
    }

    protected void ClearCharacters()
    {
        foreach (Transform child in characterParent)
            Destroy(child.gameObject);

        Characters.Clear();
    }


    public void KillAllUnsafe()
    {
        foreach(Character character in Characters)
        {
            // disabled for now, balancing ig
            /*
            if (!character.IsOnLifeBoat)
            {
                character.Die("burn");
                continue;
            }*/

            character.UnSafe("reset UI");
        }
    }
}
