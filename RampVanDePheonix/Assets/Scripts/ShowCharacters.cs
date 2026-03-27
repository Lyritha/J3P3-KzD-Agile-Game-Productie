using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowCharacters : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] Character characterPrefab;

    public List<Character> Characters { get; private set; } = new();

    private void Start()
    {
        foreach (Character character in Characters)
            Destroy(character.gameObject);

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
}
