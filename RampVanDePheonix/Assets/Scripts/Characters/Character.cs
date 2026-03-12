using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private float chanceToEat = 0.33f;
    [SerializeField]
    private CharacterDisplay characterDisplay;
    [SerializeField]
    private CharacterFood characterFood;
    [SerializeField]
    private Image borderImage;

    protected CharacterListDisplay parent;

    private Coroutine blinkRoutine;
    private static readonly Color borderColor = new Color32(0x8C, 0x73, 0x49, 0xFF);
    private static readonly Color highlightedBorderColor = new Color32(0xE6, 0xC9, 0x8A, 0xFF);

    public Personage Personage { get; private set; }
    public bool IsAlive { get; private set; } = true;
    public bool IsOnLifeBoat { get; private set; } = false;
    public bool IsSelectable => IsAlive && !IsOnLifeBoat;

    public void Initialize(Personage personage)
    {
        if (characterFood != null)
            characterFood.Initialize(this);

        this.Personage = personage;
        characterDisplay.SetUI(personage);

        this.parent = CharacterListDisplay.Instance;
    }

    public void UpdateCharacterState()
    {
        // for now 1/3 chance to consume food, can be changed to something more complex later
        bool shouldEat = Random.value < chanceToEat;
        if (shouldEat && characterFood != null) characterFood.EatFood();
    }

    public void Die(string reason = "No reason given")
    {
        IsAlive = false;
        Debug.Log($"Character {Personage.characterName} has died. Reason: {reason}");
        characterDisplay.ShowDeathScreen();
        parent.ReportCharacterDied();
        ToggleSelected(true, false);
    }

    public void Safe(string reason = "No reason given")
    {
        IsOnLifeBoat = true;
        Debug.Log($"Character {Personage.characterName} has been saved. Reason: {reason}");
        characterDisplay.ShowSafeScreen();
        ToggleSelected(true, false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsSelectable) return;

        // avoid toggling character if you can give them food.
        if (characterFood != null && characterFood.TryGiveFood()) return;

        // toggles selected state in parent object.
        ToggleSelected();
    }

    protected virtual void ToggleSelected(bool forceState = false, bool forcedState = false)
    {
        parent.SetSelectedCharacter(
            forceState ? (forcedState ? this : null) : (parent.SelectedCharacter == this ? null : this)
        );
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            blinkRoutine ??= StartCoroutine(BlinkBorder());
        }
        else
        {
            if (blinkRoutine != null)
            {
                StopCoroutine(blinkRoutine);
                blinkRoutine = null;
            }

            borderImage.color = borderColor;
        }
    }

    private IEnumerator BlinkBorder()
    {
        float speed = 5f; // higher = faster blinking

        while (true)
        {
            float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
            borderImage.color = Color.Lerp(borderColor, highlightedBorderColor, t);
            yield return null;
        }
    }
}
