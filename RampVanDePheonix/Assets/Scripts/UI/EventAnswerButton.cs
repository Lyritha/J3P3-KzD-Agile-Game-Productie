using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventAnswerButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text answerText;
    [SerializeField]
    private int answerIndex;

    private Event current;
    private EventVisualiser parent;
    CharacterListDisplay selector;
    LifeboatManager boat;
    FoodStorage foodstor;

    //adding the the
    [SerializeField] 
    Image imageding;
    [SerializeField]
    GameObject requirementPlaceholder;
    [SerializeField] GameObject defaultImagePrefab;

    bool canTriggerButtonChangeColor = true;

    public void Initialize(string answer, int index, Event currentEvent, EventVisualiser parent, EventVisualiser.IconValue[] iconValuePair)
    {
        selector = CharacterListDisplay.Instance;
        this.parent = parent;
        current = currentEvent;

        answerText.text = answer;
        answerIndex = index;

        //checks if a question has atleast a requirement or more
        if (iconValuePair.Length > 0)
        {
            InitialiseIcons(iconValuePair);
        }
    }

    private void Start()
    {
        foodstor = FindAnyObjectByType<FoodStorage>();
        boat = FindAnyObjectByType<LifeboatManager>();
    }

    public void TriggerAnswer()
    {
        //check of een personage selected is
        if (selector.SelectedCharacter != null)
        {
            //check of benodigde vaardigheden genoeg zijn
            if (current.answers[answerIndex].skillNeeded.Length > 0)
            {
                ChangeSkills(CheckSkills());
            }
            else
            {
                ChangeSkills(true);
            }
        }
        else
        {
            StartCoroutine(TemporaryChangeBackgroundColor(Color.red, 1f));
        }
    }


    /// <summary>
    /// spawns in and sets the icon that the answer requires
    /// </summary>
    void InitialiseIcons(EventVisualiser.IconValue[] iconAndValue)
    {

        //foreach requirement it makes a new icon and fills in the values
        foreach (EventVisualiser.IconValue item in iconAndValue)
        {
            GameObject newIcon = Instantiate(defaultImagePrefab);
            newIcon.transform.SetParent(requirementPlaceholder.transform);

            Image newiconImage = newIcon.GetComponent<Image>();
            newiconImage.sprite = item.iconSprite;

            TMP_Text textElement = newiconImage.GetComponentInChildren<TMP_Text>();
            textElement.text = item.amountNeeded.ToString();
        }
    }



    bool CheckSkills()
    {
        // get reference to personage to make rest more readable (and way more optimized)
        Personage personage = selector.SelectedCharacter.Personage;

        foreach (SkillNeededForAnswer skill in current.answers[answerIndex].skillNeeded)
        {
            int value = skill.skillType switch
            {
                Skillset.Kapitaal => personage.baseKapitaal,
                Skillset.Bouwkunde => personage.baseBouwkunde,
                Skillset.Socialiteit => personage.baseSociaal,
                Skillset.AanpassingsVermogen => personage.baseAanpassingsvermogen,
                Skillset.Leervermogen => personage.baseLeervermogen,
                Skillset.FoodStorage => foodstor.CurrentFood,
                _ => 0
            };

            // if any value is too low, the full check fails.
            if (value < skill.skillAmountNeeded) return false;
        }

        // if able to pass all checks, assume it is successful and return true
        return true;
    }
    void ChangeSkills(bool hasSkills)
    {
        Personage personage = selector.SelectedCharacter.Personage;

        if (hasSkills)
        {
            foreach (var change in current.answers[answerIndex].change)
            {
                switch (change.skillToBeChanged)
                {
                    case Skillset.Kapitaal:
                        personage.baseKapitaal += change.changeAmount;
                        selector.SelectedCharacter.PopUpText(personage.characterName + " Kapitaal " + change.changeAmount);
                        break;
                    case Skillset.Bouwkunde:
                        personage.baseBouwkunde += change.changeAmount;
                        selector.SelectedCharacter.PopUpText(personage.characterName + " Bouwkunde " + change.changeAmount);
                        break;
                    case Skillset.Socialiteit:
                        personage.baseSociaal += change.changeAmount;
                        selector.SelectedCharacter.PopUpText(personage.characterName + " Socialiteit " + change.changeAmount);
                        break;
                    case Skillset.AanpassingsVermogen:
                        personage.baseAanpassingsvermogen += change.changeAmount;
                        selector.SelectedCharacter.PopUpText(personage.characterName + " Aanpassingsvermogen " + change.changeAmount);
                        break;
                    case Skillset.Leervermogen:
                        personage.baseLeervermogen += change.changeAmount;
                        selector.SelectedCharacter.PopUpText(personage.characterName + " Leervermogen " + change.changeAmount);
                        break;
                    case Skillset.Death:
                        selector.SelectedCharacter.Die("ebola");
                        break;
                    case Skillset.Reddingsboot:
                        boat.SaveSelected();
                        break;
                    case Skillset.FoodStorage:
                        if (change.changeAmount < 0) foodstor.TryRemoveFood(change.changeAmount * -1);
                        else foodstor.TryAddFood(change.changeAmount);
                        break;
                    case Skillset.HungerPerPerson:
                        foreach (var character in selector.Characters)
                        {
                            int currentFoodForCheck = character.characterFood.currentFood;

                            if ((currentFoodForCheck += change.changeAmount)! < 0) character.characterFood.currentFood += change.changeAmount;
                        }
                        break;
                }
            }
        }
        else
        {
            foreach (var change in current.answers[answerIndex].changeFailed)
            {
                switch (change.skillToBeChanged)
                {
                    case Skillset.Kapitaal:
                        personage.baseKapitaal += change.changeAmount;
                        break;
                    case Skillset.Bouwkunde:
                        personage.baseBouwkunde += change.changeAmount;
                        break;
                    case Skillset.Socialiteit:
                        personage.baseSociaal += change.changeAmount;
                        break;
                    case Skillset.AanpassingsVermogen:
                        personage.baseAanpassingsvermogen += change.changeAmount;
                        break;
                    case Skillset.Leervermogen:
                        personage.baseLeervermogen += change.changeAmount;
                        break;
                    case Skillset.Death:
                        selector.SelectedCharacter.Die("ebola");
                        break;
                    case Skillset.Reddingsboot:
                        boat.SaveSelected();
                        break;
                    case Skillset.FoodStorage:
                        if (change.changeAmount < 0) foodstor.TryRemoveFood(change.changeAmount);
                        else foodstor.TryAddFood(change.changeAmount);
                        break;
                    case Skillset.HungerPerPerson:
                        foreach (var character in selector.Characters)
                        {
                            int currentFoodForCheck = character.characterFood.currentFood;

                            if ((currentFoodForCheck += change.changeAmount)! < 0) character.characterFood.currentFood += change.changeAmount;
                        }
                        break;
                }
            }
        }
        CheckForNegatives(personage);
        personage.NotifyChanged();
        string result = hasSkills ? current.answers[answerIndex].result : current.answers[answerIndex].resultFailed;
        parent.ShowResult(result);
    }

    void CheckForNegatives(Personage personage)
    {
        if (personage.baseKapitaal < 0) personage.baseKapitaal = 0;
        if (personage.baseBouwkunde < 0) personage.baseBouwkunde = 0;
        if (personage.baseSociaal < 0) personage.baseSociaal = 0;
        if (personage.baseAanpassingsvermogen < 0) personage.baseAanpassingsvermogen = 0;
        if (personage.baseLeervermogen < 0) personage.baseLeervermogen = 0;
    }


    IEnumerator TemporaryChangeBackgroundColor(Color changeToColor, float seconds)
    {
        int changeAmount = 7;
        canTriggerButtonChangeColor = false;
        //dont question it pls
        Color originalColor = imageding.color;
        for (int i = 0; i < changeAmount; i++)
        {
            imageding.color = i % 2 == 0 ? changeToColor : originalColor;
            yield return new WaitForSeconds(seconds / changeAmount);
        }
        imageding.color = originalColor;
        canTriggerButtonChangeColor = true;
        yield return new WaitForSeconds(0.1f);
    }
}
