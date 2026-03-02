using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListDisplay : MonoBehaviour
{
    
    [SerializeField] 
    private Character characterPrefab;
    [SerializeField]
    private RectTransform characterParent;

    [SerializeField]
    private CapitalDisplay capitalDisplay;

    private List<Character> characters = new();

    public void AddCharacters(List<Personage> characters) => AddCharacters(characters.ToArray());
    public void AddCharacters(Personage[] personages)
    {
        ClearCharacters();

        foreach (Personage personage in personages)
            AddCharacter(personage);

        capitalDisplay.AddCapitalItems(personages);
    }
    private void AddCharacter(Personage personage)
    {
        Character character = Instantiate(characterPrefab, characterParent);
        characters.Add(character);
        character.Initialize(personage, this);
    }



    /// <summary>
    /// trigger update on all characters, can be called every day or something like that to make characters consume food and stuff
    /// </summary>
    public void UpdateCharacters()
    {
        foreach (Character character in characters)
            if (character.IsAlive) character.UpdateCharacterState();
    }

    /// <summary>
    /// call when a character died, checks if all characters are dead and if so, shows game over screen or something similar
    /// </summary>
    public void ReportCharacterDied()
    {
        foreach (Character character in characters)
            if (character.IsAlive) return;

        // show game over screen or something similar here
        Debug.Log("All characters have died. Game Over.");
    }



    private void ClearCharacters()
    {
        foreach (Transform child in characterParent)
            Destroy(child.gameObject);

        characters.Clear();
    }
}
