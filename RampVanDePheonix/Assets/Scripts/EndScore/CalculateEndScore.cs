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
    int pointsPerCharacterAlive = 500;
    [SerializeField] bool nextPlayerStats = false;
    [SerializeField] bool nextUIElementSet = true;

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

        RemoveAllTemporaryUIElements();
        StartCoroutine(LoopTroughAlivePlayers(fakeCharacters));
    }
    /// <summary>
    /// call this method from another script to distribute points according to skillpoints and characters alive
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
                //character is alive, so points can be given for that
                mainPointCount += pointsPerCharacterAlive;
                StartCoroutine(LoopTroughSkills(character));
                yield return new WaitUntil(() => nextPlayerStats == true);
                print("waituntilCompleted");
                nextPlayerStats = false;
            }
        }
    }

    IEnumerator LoopTroughSkills(Character player)
    {
        Personage personageObject = player.Personage;
        PointClassesNames[] pointClassesToList = PointClasses.Keys.ToArray();

        for (int i = 0; i < pointClassesToList.Length; i++)
        {
            int passNumber = CheckSkill(pointClassesToList[i], personageObject);
            AddPoints(pointClassesToList[i], passNumber);
            GameObject newgameObject= PlaceNewScoreUI($"{pointClassesToList[i].ToString()} = {passNumber}");
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitUntil(() => nextUIElementSet == true);
        nextUIElementSet = false;
        print("new ui set");
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

    void AddPoints(PointClassesNames thisName, int value)
    {
        mainPointCount += value;
    }

    GameObject PlaceNewScoreUI(string text)
    {
        GameObject newUIElement = Instantiate(UItextPrefab, canvas.transform);
        ActiveTemporaryUITexts.Add(newUIElement);
        float standardPosition = 300;
        int offset = (ActiveTemporaryUITexts.Count() * 60);
        Vector3 correctPosition = new Vector3(0, (standardPosition - offset), 0);
        //we out here programming this shit
        newUIElement.transform.localPosition = correctPosition;
        FillScoreUI(newUIElement, text);
        return newUIElement;
    }

    void FillScoreUI(GameObject uiElement, string newText)
    {
        uiElement.GetComponent<TMP_Text>().text = newText;
        uiElement.GetComponent<TMP_Text>().color = Color.green;
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
