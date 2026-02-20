using TMPro;
using UnityEngine;

public class CapitalItemDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nummerText;
    [SerializeField]
    private TMP_Text naamText;
    [SerializeField]
    private TMP_Text KapitaalText;

    public Personage Character { get; private set; }

    public void SetUI(Personage character, int number)
    {
        nummerText.text = $"{number}.";
        naamText.text = $"{character.characterName}:";

        // Unsubscribe from old one (important!)
        if (this.Character != null) this.Character.OnChanged -= RefreshUI;

        this.Character = character;
        this.Character.OnChanged += RefreshUI;

        RefreshUI();
    }

    private void RefreshUI()
    {
        KapitaalText.text = Character.baseKapitaal.ToString();
    }
}
