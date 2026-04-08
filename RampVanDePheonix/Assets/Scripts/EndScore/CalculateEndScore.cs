using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CalculateEndScore : MonoBehaviour
{
    [SerializeField] WinScreen winscreen;
    [SerializeField] List<Character> characters = new List<Character>();
    [SerializeField] RectTransform statParent;


    int mainPointCount = 0;
    [SerializeField] GameObject UItextPrefab;

    //saves the main score
    GameObject mainScoreText;
    List<GameObject> ActiveTemporaryUITexts = new List<GameObject>();
    List<GameObject> ActiveScorePerPersonUI = new List<GameObject>();
    List<int> ScorePerPerson = new List<int>();

    int currentPerson = 0;
    int pointsPerCharacterAlive = 500;
    int totalPoints = 0;
    [SerializeField] bool nextPlayerStats = false;
    bool nextUIElementSet = false;


    //spawnPositions
    int defaultInstantiateXposition = -430;

    enum PointClassesNames
    {
        AanpassingsVermogen,
        Kapitaal,
        Bouwkunde,
        Socialiteit,
        Leervermogen
    }


    //dictionary based on classname, points per skillpoint
    Dictionary<PointClassesNames, int> PointClasses = new Dictionary<PointClassesNames, int>();

    public void ShowStats()
    {
        //adds a class and its base end-points per skillpoint
        PointClasses.Add(PointClassesNames.AanpassingsVermogen, 100);
        PointClasses.Add(PointClassesNames.Kapitaal, 100);
        PointClasses.Add(PointClassesNames.Bouwkunde, 100);
        PointClasses.Add(PointClassesNames.Socialiteit, 100);
        PointClasses.Add(PointClassesNames.Leervermogen, 100);

        TestDistributePoints();
    }

    /// <summary>
    /// this test if the points gets distributed correctly
    /// </summary>
    [ContextMenu("testDistribute")]
    public void TestDistributePoints()
    {
        List<Character> chars = CharacterListDisplay.Instance.Characters;
        characters.AddRange(chars);

        //checks if there is no score on screen yet (has the score already been displayed?)

        if (ActiveScorePerPersonUI.Count == 0)
            StartCoroutine(LoopTroughAlivePlayers(characters));
    }

    /// <summary>
    /// Loops trough all players, checks if the player is alive and loops trough the skills from the current character
    /// </summary>
    IEnumerator LoopTroughAlivePlayers(List<Character> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            PlaceScorePerCharacterUI(i);

            Character character = players[i];
            if (character.IsAlive == true)
            {
                //places a "main" score for the current character

                //character is alive, so points can be given for that
                mainPointCount += pointsPerCharacterAlive;
                //loops trough the skills from the current character
                StartCoroutine(LoopTroughSkills(character));
                //change nextplayerstat to true to move on to the next person stats
                yield return new WaitUntil(() => nextPlayerStats == true);
                yield return new WaitForSecondsRealtime(1f);
                //after waiting, it prepares for the next player
            }
            else
            {
                nextUIElementSet = false;
            }

            currentPerson++;
            nextPlayerStats = false;
        }

        winscreen.ToggleButton(true);
    }

    /// <summary>
    /// method loops trough all the skills from the current player
    /// method also adds points and places "mini" scores per skillpoint-item
    /// </summary>
    IEnumerator LoopTroughSkills(Character player)
    {
        Personage personageObject = player.Personage;
        PointClassesNames[] pointClassesToList = PointClasses.Keys.ToArray();

        for (int i = 0; i < pointClassesToList.Length; i++)
        {
            int passNumber = CheckSkill(pointClassesToList[i], personageObject);
            AddPoints(pointClassesToList[i], passNumber);
            PlaceNewScoreUI($"{pointClassesToList[i]}: {passNumber}");

            yield return new WaitForSecondsRealtime(0.5f);
        }

        nextPlayerStats = true;
        nextUIElementSet = false;
    }

    /// <summary>
    /// checks the amount of skillpoints, multiplies that by the amount of points and returns that amount (to be added to the characterUIscore)
    /// </summary>
    int CheckSkill(PointClassesNames pointclass, Personage personageObject)
    {
        if (PointClasses.TryGetValue(pointclass, out int value))
        {
            int addPoints = pointclass switch
            {
                PointClassesNames.AanpassingsVermogen => (personageObject.baseAanpassingsvermogen * value),
                PointClassesNames.Bouwkunde => (personageObject.baseBouwkunde * value),
                PointClassesNames.Kapitaal => (personageObject.baseKapitaal * value),
                PointClassesNames.Socialiteit => (personageObject.baseSociaal * value),
                PointClassesNames.Leervermogen => (personageObject.baseLeervermogen * value),
                _ => 0,
            };
            return addPoints;
        }
        else
        {
            print("nameNotFoundInDictionary");
            return 0;
        }
    }

    /// <summary>
    /// adds points to the score from current selected character 
    /// </summary>
    void AddPoints(PointClassesNames thisName, int value)
    {
        ScorePerPerson[currentPerson] += value;
        totalPoints += value;
        winscreen.UpdateScore(totalPoints);

        //this is a debug text, can be removed
        //print($"{thisName} adds {value} points to {characters[currentPerson].Personage.characterName}");

        //updates the "main" score per character
        UpdateScorePerCharacterUI(currentPerson);
    }

    /// <summary>
    /// Places a main-score per character
    /// PS: the "mini" scores (score per skillponts) all gets added up, and placed into this UI element
    /// </summary>
    void PlaceScorePerCharacterUI(int index)
    {
        //confirm position before adding new GameObject to the list
        float standardYPosition = 300f;
        float stepPerUIperson = 250;
        float xOffset = (defaultInstantiateXposition + ActiveScorePerPersonUI.Count * stepPerUIperson);

        //instantiates the characterScoreUI
        GameObject characterScore = Instantiate(UItextPrefab, statParent.GetChild(index));

        //puts it in the scoreUI per person
        ActiveScorePerPersonUI.Add(characterScore);

        //places it in the correct position
        Vector3 position = new Vector3(xOffset, standardYPosition, 0);
        characterScore.transform.localPosition = position;
        //sets the base score to 0, removing this causes a null reference since no objects otherwise exist in the list
        ScorePerPerson.Add(0);
        //fills the UI text

        TMP_Text text = characterScore.GetComponentInChildren<TMP_Text>();
        text.text = ScorePerPerson[currentPerson].ToString();
        text.fontSize = 36;
    }


    /// <summary>
    ///  updates the scoreUI based on the current (number) character
    /// </summary>
    void UpdateScorePerCharacterUI(int index)
    {
        int score = ScorePerPerson[index];
        TMP_Text text = ActiveScorePerPersonUI[index].GetComponentInChildren<TMP_Text>();
        text.text = score.ToString();
    }


    /// <summary>
    ///  places a new score UI and places it on the correct position
    ///  after instantiating it calls fillScoreUI to fill the newly made UI element
    /// </summary>
    GameObject PlaceNewScoreUI(string text)
    {
        float standardYPosition = 200f;
        float stepPerUIperson = 250;
            //calculates the offset for the UI element,
            //activescoreperperon -1 because an scoreElement per person already gets instantiated causing misalignment
            //tldr score per character gets instantiated faster than placenewScoreUI
        float xOffset = (defaultInstantiateXposition + (ActiveScorePerPersonUI.Count - 1) * stepPerUIperson);
        int yOffset = (ActiveTemporaryUITexts.Count() * 75);

        //instantiating the new UI element
        GameObject newUIElement = Instantiate(UItextPrefab, statParent.GetChild(currentPerson));

        //puts it in the TempUI list
        ActiveTemporaryUITexts.Add(newUIElement);

        //changes the position - the offsets
        Vector3 position = new Vector3(xOffset, (standardYPosition - yOffset), 0);
        newUIElement.transform.localPosition = position;

        //fills the score with the passed trough text
        newUIElement.GetComponentInChildren<TMP_Text>().text = text;
        return newUIElement;
    }
}
