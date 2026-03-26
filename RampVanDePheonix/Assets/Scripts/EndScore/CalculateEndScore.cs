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
    [SerializeField] List<GameObject> ActiveTemporaryUITexts = new List<GameObject>();
    [SerializeField] List<GameObject> ActiveScorePerPersonUI = new List<GameObject>();
    [SerializeField] List<int> ScorePerPerson = new List<int>();

    int currentPerson = 0;
    int pointsPerCharacterAlive = 500;
    [SerializeField] bool nextPlayerStats = false;
    [SerializeField] bool nextUIElementSet = false;
    [SerializeField] bool isBusyDisplaying = false;

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

    IEnumerator LoopTroughAlivePlayers(List<Character> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            RemoveAllTemporaryUIElements();
            Character character = players[i];
            if (character.IsAlive == true)
            {
                placeScorePerCharacterUI();
                //character is alive, so points can be given for that
                mainPointCount += pointsPerCharacterAlive;
                StartCoroutine(LoopTroughSkills(character));
                //change nextplayerstat to true to move on to the next person stats
                yield return new WaitUntil(() => nextPlayerStats == true);
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

    void PrepareForNextPlayer()
    {
        RemoveAllTemporaryUIElements();
        currentPerson++;
        nextPlayerStats = false;
    }
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
    /// checks the amount of skillpoints, multiplies that by the amount of points and adds that to the total point count
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
        print($"{thisName} adds {value} points to {fakeCharacters[currentPerson].Personage.characterName}");
        updateScorePerCharacterUI(currentPerson);
    }

    void placeScorePerCharacterUI()
    {
        //confirm position before adding new GameObject to the list
        float standardYPosition = 300f;
        float stepPerUIperson = 250;
        float xOffset = (defaultInstantiateXposition + ActiveScorePerPersonUI.Count * stepPerUIperson);
        GameObject characterScore = Instantiate(UItextPrefab, canvas.transform);
        ActiveScorePerPersonUI.Add(characterScore);

        Vector3 position = new Vector3(xOffset, standardYPosition, 0);
        characterScore.transform.localPosition = position;
        //sets the base score to 0, removing this causes a null reference since no objects otherwise exist in the list
        ScorePerPerson.Add(0);
        FillScoreUI(characterScore, ScorePerPerson[currentPerson].ToString());
    }

    void updateScorePerCharacterUI(int index)
    {
        int score = ScorePerPerson[index];
        ActiveScorePerPersonUI[index].GetComponentInChildren<TMP_Text>().text = score.ToString();
    }



    GameObject PlaceNewScoreUI(string text)
    {
        float standardYPosition = 200f;
        float stepPerUIperson = 250;
        //calculates the offset for the UI element,
        //activescoreperperon -1 because an scoreElement per person already gets instantiated causing misalignment
        //tldr score per character gets instantiated faster than placenewScoreUI
        float xOffset = (defaultInstantiateXposition + (ActiveScorePerPersonUI.Count - 1) * stepPerUIperson);
        int yOffset = (ActiveTemporaryUITexts.Count() * 60);
        GameObject newUIElement = Instantiate(UItextPrefab, canvas.transform);
        ActiveTemporaryUITexts.Add(newUIElement);
        Vector3 position = new Vector3(xOffset, (standardYPosition - yOffset), 0);
        newUIElement.transform.localPosition = position;
        FillScoreUI(newUIElement, text);
        return newUIElement;
    }

    void FillScoreUI(GameObject uiElement, string newText)
    {
        uiElement.GetComponentInChildren<TMP_Text>().text = newText;
        uiElement.GetComponentInChildren<TMP_Text>().color = Color.green;
    }

    void RemoveAllTemporaryUIElements()
    {
        foreach (GameObject element in ActiveTemporaryUITexts)
        {
            GameObject.Destroy(element);
        }
        ActiveTemporaryUITexts.Clear();
    }

    void RemoveItemTemporaryUIElement(GameObject itemToDelete)
    {
        GameObject.Destroy(itemToDelete);
        ActiveTemporaryUITexts.Remove(itemToDelete);
    }
}
