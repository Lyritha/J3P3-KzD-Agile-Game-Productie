using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class CalculateEndScore : MonoBehaviour
{
    [SerializeField] List<Character> fakeCharacters = new List<Character>();
    int mainPointCount = 0;
    [SerializeField] GameObject UItextPrefab;
    [SerializeField] Canvas canvas;
    //saves the main score
    GameObject mainScoreText;
    List<GameObject> ActiveTemporaryUITexts = new List<GameObject>();
    List<GameObject> ActiveScorePerPersonUI = new List<GameObject>();
    List<int> ScorePerPerson = new List<int>();

    int currentPerson = 0;
    int pointsPerCharacterAlive = 500;
    [SerializeField] bool nextPlayerStats = false;
    bool nextUIElementSet = false;
    bool isBusyDisplaying = false;

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

    void Start()
    {
        //adds a class and its base end-points per skillpoint
        PointClasses.Add(PointClassesNames.AanpassingsVermogen, 100);
        PointClasses.Add(PointClassesNames.Kapitaal, 100);
        PointClasses.Add(PointClassesNames.Bouwkunde, 100);
        PointClasses.Add(PointClassesNames.Socialiteit, 100);
        PointClasses.Add(PointClassesNames.Leervermogen, 100);
    }

    /// <summary>
    /// this test if the points gets distributed correctly
    /// </summary>
    [ContextMenu("testDistribute")]
    public void TestDistributePoints()
    {
        List<Character> chars = CharacterListDisplay.Instance.Characters;
        fakeCharacters.Add(chars[0]);
        fakeCharacters.Add(chars[1]);
        fakeCharacters.Add(chars[2]);
        fakeCharacters.Add(chars[3]);

        RemoveAllTemporaryUIElements();
        //checks if there is no score on screen yet (has the score already been displayed?)
        if (ActiveScorePerPersonUI.Count == 0)
        {
            StartCoroutine(LoopTroughAlivePlayers(fakeCharacters));
        }
    }
    /// <summary>
    /// call this method from another script to distribute points according to skillpoints and characters alive
    /// NOTE: call this methode once and continue with NextPlayer()
    /// </summary>
    public void StartDistributingPoints(List<Character> players)
    {
        RemoveAllTemporaryUIElements();
        StartCoroutine(LoopTroughAlivePlayers(players));
    }

    /// <summary>
    /// Loops trough all players, checks if the player is alive and loops trough the skills from the current character
    /// </summary>
    IEnumerator LoopTroughAlivePlayers(List<Character> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            //removes all UI elements
            RemoveAllTemporaryUIElements();
            Character character = players[i];
            if (character.IsAlive == true)
            {
                //places a "main" score for the current character
                placeScorePerCharacterUI();
                //character is alive, so points can be given for that
                mainPointCount += pointsPerCharacterAlive;
                //loops trough the skills from the current character
                StartCoroutine(LoopTroughSkills(character));
                //change nextplayerstat to true to move on to the next person stats
                yield return new WaitUntil(() => nextPlayerStats == true);
                //after waiting, it prepares for the next player
                PrepareForNextPlayer();
            }

        }
    }

    /// <summary>
    /// call this method to continue to next player (for example a button)
    /// </summary>
    [ContextMenu("nextPlayer")]
    public void NextPlayer()
    {
        if (isBusyDisplaying == false)
        {
            nextPlayerStats = true;
        }
    }

    /// <summary>
    /// prepares the UI screen by removing all temporary items and going to the next person
    /// </summary>
    void PrepareForNextPlayer()
    {
        RemoveAllTemporaryUIElements();
        currentPerson++;
        nextPlayerStats = false;
    }

    /// <summary>
    /// method loops trough all the skills from the current player
    /// method also adds points and places "mini" scores per skillpoint-item
    /// </summary>
    IEnumerator LoopTroughSkills(Character player)
    {
        isBusyDisplaying = true;
        Personage personageObject = player.Personage;
        PointClassesNames[] pointClassesToList = PointClasses.Keys.ToArray();

        for (int i = 0; i < pointClassesToList.Length; i++)
        {
            int passNumber = CheckSkill(pointClassesToList[i], personageObject);
            AddPoints(pointClassesToList[i], passNumber);
            GameObject newgameObject = PlaceNewScoreUI($"{pointClassesToList[i].ToString()} = {passNumber}");
            yield return new WaitForSeconds(0.5f);
        }
        isBusyDisplaying = false;
        nextUIElementSet = false;
    }

    /// <summary>
    /// checks the amount of skillpoints, multiplies that by the amount of points and returns that amount (to be added to the characterUIscore)
    /// </summary>
    int CheckSkill(PointClassesNames pointclass, Personage personageObject)
    {
        if (PointClasses.TryGetValue(pointclass, out int value))
        {
            int addPoints = 0;
            switch (pointclass)
            {
                case PointClassesNames.AanpassingsVermogen:
                    addPoints = (personageObject.baseAanpassingsvermogen * value);
                    break;
                case PointClassesNames.Bouwkunde:
                    addPoints = (personageObject.baseBouwkunde * value);
                    break;
                case PointClassesNames.Kapitaal:
                    addPoints = (personageObject.baseKapitaal * value);
                    break;
                case PointClassesNames.Socialiteit:
                    addPoints = (personageObject.baseSociaal * value);
                    break;
                case PointClassesNames.Leervermogen:
                    addPoints = (personageObject.baseLeervermogen * value);
                    break;
                default:
                    addPoints = 0;
                    break;
            }
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
        //this is a debug text, can be removed
        print($"{thisName} adds {value} points to {fakeCharacters[currentPerson].Personage.characterName}");

        //updates the "main" score per character
        updateScorePerCharacterUI(currentPerson);
    }

    /// <summary>
    /// Places a main-score per character
    /// PS: the "mini" scores (score per skillponts) all gets added up, and placed into this UI element
    /// </summary>
    void placeScorePerCharacterUI()
    {
        //confirm position before adding new GameObject to the list
        float standardYPosition = 300f;
        float stepPerUIperson = 250;
        float xOffset = (defaultInstantiateXposition + ActiveScorePerPersonUI.Count * stepPerUIperson);

        //instantiates the characterScoreUI
        GameObject characterScore = Instantiate(UItextPrefab, canvas.transform);

        //puts it in the scoreUI per person
        ActiveScorePerPersonUI.Add(characterScore);

        //places it in the correct position
        Vector3 position = new Vector3(xOffset, standardYPosition, 0);
        characterScore.transform.localPosition = position;
        //sets the base score to 0, removing this causes a null reference since no objects otherwise exist in the list
        ScorePerPerson.Add(0);
        //fills the UI text
        FillScoreUI(characterScore, ScorePerPerson[currentPerson].ToString());
    }


    /// <summary>
    ///  updates the scoreUI based on the current (number) character
    /// </summary>
    void updateScorePerCharacterUI(int index)
    {
        int score = ScorePerPerson[index];
        ActiveScorePerPersonUI[index].GetComponentInChildren<TMP_Text>().text = score.ToString();
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
        GameObject newUIElement = Instantiate(UItextPrefab, canvas.transform);

        //puts it in the TempUI list
        ActiveTemporaryUITexts.Add(newUIElement);

        //changes the position - the offsets
        Vector3 position = new Vector3(xOffset, (standardYPosition - yOffset), 0);
        newUIElement.transform.localPosition = position;

        //fills the score with the passed trough text
        FillScoreUI(newUIElement, text);
        return newUIElement;
    }

    /// <summary>
    /// fills the UI with text (both passed trough)
    /// </summary>
    void FillScoreUI(GameObject uiElement, string newText)
    {
        uiElement.GetComponentInChildren<TMP_Text>().text = newText;
        uiElement.GetComponentInChildren<TMP_Text>().color = Color.green;
    }

    /// <summary>
    /// removes all the temporary UI items (such as basic scores)
    /// </summary>
    void RemoveAllTemporaryUIElements()
    {
        foreach (GameObject element in ActiveTemporaryUITexts)
        {
            GameObject.Destroy(element);
        }
        ActiveTemporaryUITexts.Clear();
    }

    /// <summary>
    /// removes a single item from the temporary UI items (such as a basic score)
    /// </summary>
    void RemoveItemTemporaryUIElement(GameObject itemToDelete)
    {
        GameObject.Destroy(itemToDelete);
        ActiveTemporaryUITexts.Remove(itemToDelete);
    }

    /// <summary>
    /// use this method to correctly pass the canvas where the items can be displayed
    /// </summary>
    public void SetCanvas(Canvas correctCanvas)
    {
        canvas = correctCanvas;
    }
}
