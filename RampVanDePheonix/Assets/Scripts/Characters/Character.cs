using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private CharacterDisplay characterDisplay;
    [SerializeField]
    private CharacterFood characterFood;
    [SerializeField]
    private Image borderImage;

    private Personage personage;
    private CharacterListDisplay parent;

    private static readonly Color borderColor = new Color32(0x8C, 0x73, 0x49, 0xFF);
    private static readonly Color highlightedBorderColor = new Color32(0xB0, 0x94, 0x62, 0xFF);

    public bool IsAlive { get; private set; } = true;
    public bool IsOnLifeBoat { get; private set; } = false;
    public void Initialize(Personage personage, CharacterListDisplay parent)
    {
        characterFood.Initialize(this);
        this.personage = personage;
        characterDisplay.SetUI(personage);
        this.parent = parent;
    }

    public void UpdateCharacterState()
    {
        // for now 50% chance to consume food, can be changed to something more complex later
        bool shouldEat = Random.value < 0.5f;
        if (shouldEat) characterFood.EatFood();
    }

    public void Die(string reason = "No reason given")
    {
        IsAlive = false;
        Debug.Log($"Character {personage.characterName} has died. Reason: {reason}");
        characterDisplay.ShowDeathScreen();
        parent.ReportCharacterDied();
        ToggleSelected();
    }

    public void Safe(string reason = "No reason given")
    {
        IsOnLifeBoat = true;
        Debug.Log($"Character {personage.characterName} has been saved. Reason: {reason}");
        characterDisplay.ShowSafeScreen();
        ToggleSelected();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsAlive || IsOnLifeBoat) return;
        ToggleSelected();
    }

    private void ToggleSelected()
    {
        parent.SetSelectedCharacter(
            parent.SelectedCharacter == this ? null : this
        );
    }

    public void SetSelected(bool isSelected) => borderImage.color = isSelected ? highlightedBorderColor : borderColor;
}
