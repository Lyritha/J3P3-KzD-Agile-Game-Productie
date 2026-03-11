using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{

    [Foldout("Small display", true), SerializeField]
    private TMP_Text displayNaam;
    [SerializeField]
    private Image displaySprite;

    [Foldout("Info dropdown", true), SerializeField]
    private int maxSliderValue = 5;
    [SerializeField]
    private TMP_Text infoNaam;
    [SerializeField]
    private Image infoSprite;
    [SerializeField]
    private TMP_Text infoBeroep;
    [SerializeField]
    private TMP_Text infoWoonplaats;
    [SerializeField]
    private TMP_Text infoLore;

    [Header("Slider"),SerializeField]
    private Slider bouwkundeSlider;
    [SerializeField]
    private Slider leervermogenSlider;
    [SerializeField]
    private Slider aanpassingsvermogenSlider;
    [SerializeField]
    private Slider sociaalSlider;
    [SerializeField]
    private TMP_Text kapitaalText;

    [Header("Slider"), SerializeField]
    private TMP_Text bouwkundeText;
    [SerializeField]
    private TMP_Text leervermogenText;
    [SerializeField]
    private TMP_Text aanpassingsvermogenText;
    [SerializeField]
    private TMP_Text sociaalText;

    [Foldout("Death display", true), SerializeField]
    private ShowOnHover showOnHover;
    [SerializeField]
    private RectTransform deathScreen;
    [SerializeField]
    private RectTransform safeScreen;

    // dunno about hunger yet uwu

    private Personage character;

    public void SetUI(Personage character)
    {
        bouwkundeSlider.maxValue = maxSliderValue;
        leervermogenSlider.maxValue = maxSliderValue;
        aanpassingsvermogenSlider.maxValue = maxSliderValue;
        sociaalSlider.maxValue = maxSliderValue;

        // Unsubscribe from old one (important!)
        if (this.character != null) this.character.OnChanged -= RefreshUI;

        this.character = character;
        this.character.OnChanged += RefreshUI;

        RefreshUI();
    }


    private void RefreshUI()
    {
        displayNaam.text = character.characterName;
        displaySprite.sprite = character.portrait;

        infoNaam.text = character.characterName;
        infoSprite.sprite = character.portrait;

        infoBeroep.text = character.beroep;
        infoWoonplaats.text = character.woonplaats;

        bouwkundeSlider.value = character.baseBouwkunde;
        leervermogenSlider.value = character.baseLeervermogen;
        aanpassingsvermogenSlider.value = character.baseAanpassingsvermogen;
        sociaalSlider.value = character.baseSociaal;
        kapitaalText.text = character.baseKapitaal.ToString();

        bouwkundeText.text = $"{character.baseBouwkunde}/{maxSliderValue}";
        leervermogenText.text = $"{character.baseLeervermogen}/{maxSliderValue}";
        aanpassingsvermogenText.text = $"{character.baseAanpassingsvermogen}/{maxSliderValue}";
        sociaalText.text = $"{character.baseSociaal}/{maxSliderValue}";

        infoLore.text = character.loreDrop;
    }

    public void ShowDeathScreen()
    {
        deathScreen.gameObject.SetActive(true);
        showOnHover.SetEnabled(false);
    }

    public void ShowSafeScreen()
    {
        safeScreen.gameObject.SetActive(true);
        showOnHover.SetEnabled(false);
    }
}
