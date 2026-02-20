using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListDisplay : MonoBehaviour
{
    public List<CharacterDisplay> Characters { get; private set; } = new();
    
    [SerializeField] 
    private CharacterDisplay characterPrefab;
    [SerializeField]
    private RectTransform characterParent;

    [SerializeField]
    private CapitalDisplay capitalDisplay;


    [ContextMenu("Add Manual Characters")]  
    private void AddManualCharacters()
    {
        PartyGen partyGen = FindAnyObjectByType<PartyGen>();
        if (partyGen != null) AddCharacters(partyGen.party);
    }

    public void AddCharacters(List<Personage> characters) => AddCharacters(characters.ToArray());
    public void AddCharacters(Personage[] characters)
    {
        ClearCharacters();

        foreach (Personage character in characters)
            AddCharacter(character);

        capitalDisplay.AddCapitalItems(characters);
    }

    private void AddCharacter(Personage character)
    {
        CharacterDisplay display = Instantiate(characterPrefab, characterParent);
        Characters.Add(display);
        display.SetUI(character);
    }

    private void ClearCharacters()
    {
        foreach (Transform child in characterParent)
            Destroy(child.gameObject);

        Characters.Clear();
    }
}
