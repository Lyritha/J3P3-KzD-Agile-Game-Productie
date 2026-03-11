using Unity.VisualScripting;
using UnityEngine.UI;

public class CharacterListDisplayTut : CharacterListDisplay
{
    public override void AddCharacters(Personage[] personages)
    {
        ClearCharacters();

        for (int i = 0; i < personages.Length; i++)
        {
            Personage personage = personages[i];

            // first element
            if (i == 0) AddCharacterFirst(personage);
            else AddCharacter(personage);
        }


        capitalDisplay.AddCapitalItems(personages);
    }

    protected void AddCharacterFirst(Personage personage)
    {
        Character character = Instantiate(characterPrefab, characterParent);
        TutorialButton tutorialButton = character.AddComponent<TutorialButton>();
        tutorialButton.SetTag("Koop eten karakter");

        Button button = character.GetComponent<Button>();
        button.onClick.AddListener(tutorialButton.TriggerButtonNext);

        Characters.Add(character);
        character.Initialize(personage, this);
    }
}
