using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private CharacterDisplay characterDisplay;
    [SerializeField]
    private CharacterFood characterFood;

    private Personage personage;
    private CharacterListDisplay parent;

    public bool IsAlive { get; private set; } = true;

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
        if (shouldEat) characterFood.ConsumeFood();
    }

    public void Die(string reason = "No reason given")
    {
        IsAlive = false;
        Debug.Log($"Character {personage.characterName} has died. Reason: {reason}");
        characterDisplay.ShowDeathScreen();
        parent.ReportCharacterDied();
    }


}
