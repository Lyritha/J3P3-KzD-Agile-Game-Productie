using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CalculateEndScore : MonoBehaviour
{

    int mainPointCount = 0;
    [SerializeField] GameObject UItextPrefab;
    //saves the main score
    GameObject mainScoreText;
    List<GameObject> ActiveTemporaryUITexts = new List<GameObject>();
    int pointsPerCharacterAlive = 500;

    enum PointClassesNames
    {
        AanpassingsVermogen,
        Kapitaal,
        Bouwkunde,
        Socialiteit,
        Leervermogen
    }


    //dictionary based on classname, points per skillpoint
    [SerializeField] Dictionary<PointClassesNames, int> PointClasses = new Dictionary<PointClassesNames, int>();

    void Start()
    {
        //adds a class and its base end-points per skillpoint
        PointClasses.Add(PointClassesNames.AanpassingsVermogen, 100);
        PointClasses.Add(PointClassesNames.Kapitaal, 100);
        PointClasses.Add(PointClassesNames.Bouwkunde, 100);
        PointClasses.Add(PointClassesNames.Socialiteit, 100);
        PointClasses.Add(PointClassesNames.Leervermogen, 100);
    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// call this method from another script to distribute points according to skillpoints and characters alive
    /// </summary>
    public void StartDistributingPoints(Character[] players)
    {

    }

    void LoopTroughPlayers(Character[] players)
    {
        foreach (Character character in players)
        {
            if (character.IsAlive == true)
            {
                //character is alive, so points can be given for that
                mainPointCount += pointsPerCharacterAlive;
                LoopTroughSkills(character);
            }
        }
    }

    void LoopTroughSkills(Character player)
    {
        Personage personageObject = player.Personage;

        CheckSkill(PointClassesNames.AanpassingsVermogen, personageObject);
        CheckSkill(PointClassesNames.Bouwkunde, personageObject);
        CheckSkill(PointClassesNames.Kapitaal, personageObject);
        CheckSkill(PointClassesNames.Socialiteit, personageObject);
        CheckSkill(PointClassesNames.Leervermogen, personageObject);
    }

    void CheckSkill(PointClassesNames pointclass, Personage personageObject)
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
            AddPoints(pointclass, value);
        }
        else
        {
            print("nameNotFoundInDictionary");
        }
    }

    void AddPoints(PointClassesNames thisName, int value)
    {
        PlaceNewScoreUI($"{thisName.ToString()} = {value}");
    }

    void PlaceNewScoreUI(string text)
    {
        GameObject newUIElement = Instantiate(UItextPrefab);
        FillScoreUI(newUIElement, text);
    }

    void FillScoreUI(GameObject uiElement, string text)
    {

    }

    void RemoveAllTemporaryUIElements()
    {
        foreach (GameObject element in ActiveTemporaryUITexts)
        {
            GameObject.Destroy(element);
        }
        ActiveTemporaryUITexts.Clear();
    }
}
