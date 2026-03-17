using MyBox;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//pls do not open this script as a code example

public class UIQuestionManager : MonoBehaviour
{
    [SerializeField] MinigameFinished minigameFinished;

    [SerializeField] GameObject TimerManager;

    [SerializeField] GameObject UIcardAnswerPrefab;
    [SerializeField] GameObject UIcardQuestionPrefab;
    [SerializeField] RectTransform UIPointsCardPrefab;
    [SerializeField] RectTransform Canvas;
    [SerializeField] TMP_Text AmountOfPointsText;
    [SerializeField] RectTransform answerPanel;
    [SerializeField] RectTransform questionParent;
    [SerializeField] RectTransform answerParent;

    BaseScriptable[] questionsForBoat;
    BaseScriptable[] questionsForAmerica;
    BaseScriptable currentQuestion;
    public Fases currentFase = Fases.Boat;

    int currentAmountOfPoints = 0;
    int amountOfPointsPerQuestion = 1;

    //get the actual player stat from the player and place it in this variable pls, this value is just for testing
    public int testPlayerStat = 5;

    int maxRemoveWord = 3;
    int maxStat = 5;
    bool EventEnded = false;

    List<RectTransform> activeCards;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        AmountOfPointsText.text = currentAmountOfPoints.ToString();
        questionsForBoat = Resources.LoadAll<BaseScriptable>("FaseBoatQuestions");
        questionsForAmerica = Resources.LoadAll<BaseScriptable>("FaseAmericaQuestions");
        activeCards = new List<RectTransform>();
        NewQuestion(0);
        InitialiseWorld(); //work on this if characters, scenery etc are done
    }

    private void Update()
    {
        if (EventEnded == false)
        {
            minigameFinished.ShowScore(currentAmountOfPoints);
            CheckIfTimerIsDone(TimerManager.GetComponent<Timer>());
        }
    }
    //read scriptable > create new UI element > create cards.

    //gets a random new question
    void NewQuestion()
    {
        switch (currentFase)
        {
            case Fases.Boat:
                ReadNewScriptable(questionsForBoat);
                break;
            case Fases.America:
                ReadNewScriptable(questionsForAmerica);
                break;
        }
        EmptyActiveList();
        SpawnNewCards(currentQuestion);
    }
    //get a chosen new question (not random)
    void NewQuestion(int questionNumber)
    {
        switch (currentFase)
        {
            case Fases.Boat:
                ReadNewScriptable(questionsForBoat, questionNumber);
                break;
            case Fases.America:
                ReadNewScriptable(questionsForAmerica, questionNumber);
                break;
        }
        EmptyActiveList();
        SpawnNewCards(currentQuestion);
    }
    //reads a random scriptable object
    void ReadNewScriptable(BaseScriptable[] chosenFase)
    {
        int randomNumber = UnityEngine.Random.Range(0, chosenFase.Length);
        currentQuestion = chosenFase[randomNumber];
    }

    //reads a chosen scriptable object
    void ReadNewScriptable(BaseScriptable[] chosenFase, int Number)
    {
        currentQuestion = chosenFase[Number];
    }
    void SpawnNewCards(BaseScriptable currentQuestion)
    {
        activeCards.Add(placeQuestionCard(currentQuestion));
        activeCards.AddRange(PlaceAnswerCards(currentQuestion));
    }

    RectTransform placeQuestionCard(BaseScriptable currentQuestion)
    {
        RectTransform questionGameObject = Instantiate(UIcardQuestionPrefab).GetComponent<RectTransform>();
        questionGameObject.GetComponent<Button>().enabled = false;
        questionGameObject.SetParent(questionParent, false);

        questionGameObject.GetComponentInChildren<TMP_Text>().text = ReplaceWords(currentQuestion.AmericanManSays, testPlayerStat);
        return questionGameObject;
    }
    List<RectTransform> PlaceAnswerCards(BaseScriptable currentQuestion)
    {
        answer[] allPossibleAnswers = currentQuestion.allAnswers;
        List<RectTransform> newlyMadeCards = new List<RectTransform>();

        for (int i = 0; i < allPossibleAnswers.Length; i++)
        {
            answer currentAnswer = allPossibleAnswers[i];
            RectTransform newCard = Instantiate(UIcardAnswerPrefab).GetComponent<RectTransform>();
            newCard.SetParent(answerPanel);
            newCard.GetComponentInChildren<TMP_Text>().text = currentAnswer.anAnswer;

            Button button = newCard.gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => GiveAnswer(currentAnswer));

            newlyMadeCards.Add(newCard);
        }
        return newlyMadeCards;
    }

    public void GiveAnswer(answer answer)
    {
        if (answer.isCorrect == true)
        {
            ChangePoints(amountOfPointsPerQuestion);
        }
        else
        {
            ChangePoints(-amountOfPointsPerQuestion);
        }
        SetTextToCanvas(currentAmountOfPoints);
        EmptyActiveList();
        PlacePlayerChosenAnswerCard(answer);
        StartCoroutine(ActivateNewQuestion());
    }

    void ChangePoints(int amountToChange)
    {
        currentAmountOfPoints += amountToChange;
        StartCoroutine(ShowQuickPointChange(amountToChange));
    }
    void EmptyActiveList()
    {
        foreach (RectTransform benjaminNetanyahu in activeCards)
        {
            Destroy(benjaminNetanyahu.gameObject);
        }
        activeCards.Clear();
    }

    void PlacePlayerChosenAnswerCard(answer answer)
    {
        RectTransform questionGameObject = Instantiate(UIcardQuestionPrefab).GetComponent<RectTransform>();
        questionGameObject.GetComponent<Button>().enabled = false;
        questionGameObject.SetParent(answerParent, false);
        questionGameObject.GetComponentInChildren<TMP_Text>().text = answer.anAnswer;
        activeCards.Add(questionGameObject);
    }
    public enum Fases
    {
        Boat,
        America
    }

    string ReplaceWords(string question, int playerLearnStat)
    {
        string endString = "";
        //calculates the % of words that the player knows (kinda simulated)
        float playerKnowledge = (playerLearnStat + 1) / maxStat * 100;
        float unknownWordPercentage = 100 - playerKnowledge;

        //calculates how many words need to be removed according to Player stat (higher learn ability means less words removed)
        int RemoveAmountOfWords = (int)((unknownWordPercentage / 100) * maxRemoveWord);
        //split the sentence in words
        string[] words = question.Split(' ');

        //gets random word out of the sentence and "writes" over them
        for (int i = 0; i < RemoveAmountOfWords; i++)
        {
            //colling code, makes new string with "*" char with the lenght of the original word
            int currentRandom = UnityEngine.Random.Range(1, words.Length);
            string output = "";
            output += new string('*', words[currentRandom].Length);
            words[currentRandom] = output;
        }

        //rebuild sentence
        foreach (string word in words)
        {
            //get each word and place it in the endstring
            endString += word + " ";
        }
        return endString;
    }

    void SetTextToCanvas(int currentPoints)
    {
        string extraText = $"je hebt nu {currentAmountOfPoints} punten";
        AmountOfPointsText.text = extraText;
    }

    IEnumerator ActivateNewQuestion()
    {
        //waiting a few seconds after answering
        yield return new WaitForSeconds(2);
        NewQuestion();
    }

    void CheckIfTimerIsDone(Timer timerScript)
    {
        if (timerScript.amountOfSeconds < 0)
        {

            EmptyActiveList();
            print(TellEndScore().ToString());
            EventEnded = true;
        }
    }

    //use this method to recieve the score
    public int TellEndScore() => CalculateActualSkillPointIncrease(currentAmountOfPoints);

    int CalculateActualSkillPointIncrease(int currentPoints)
    {
        int maxPointIncrease = 3;
        int TargetForMaxPoint = 6;

        float calculatedPoints = (float)((float)currentAmountOfPoints / (float)TargetForMaxPoint) * (float)maxPointIncrease;
        int actualReturn = (calculatedPoints.RoundToInt()).Clamp(0, maxPointIncrease);

        return actualReturn;
    }
    IEnumerator ShowQuickPointChange(int amountOfPoints)
    {
        RectTransform pointCard = Instantiate(UIPointsCardPrefab);
        pointCard.SetParent(Canvas, false);
        int amountIterations = 180;
        float secondsActive = 2f;
        TMP_Text cardTMP = pointCard.GetComponent<TMP_Text>();
        cardTMP.color = Color.red;
        if (amountOfPoints > 0)
        {
            cardTMP.color = Color.green;
        }
        cardTMP.text = amountOfPoints.ToString();

        for (int i = 0; i < amountIterations; i++)
        {
            Vector3 currentPos = pointCard.transform.position;
            currentPos.y += 7;
            pointCard.transform.position = currentPos;
            yield return new WaitForSeconds((secondsActive / amountIterations));
        }
        GameObject.Destroy(pointCard.gameObject);
    }

    void InitialiseWorld()
    {
        //set background
        //set characters
        //get fase (america or boat)
    }
}

