using MyBox;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestionManager : MonoBehaviour
{
    [SerializeField] GameObject UIcardAnswerPrefab;
    [SerializeField] GameObject UIcardQuestionPrefab;
    [SerializeField] Canvas Canvas;
    [SerializeField] TMP_Text AmountOfPointsText;
    [SerializeField] RectTransform answerParent;
    [SerializeField] RectTransform questionParent;

    BaseScriptable[] questionsForBoat;
    BaseScriptable[] questionsForAmerica;
    BaseScriptable currentQuestion;
    public Fases currentFase = Fases.Boat;

    int currentAmountOfPoints = 0;

    //get the actual player stat from the player and place it in this variable pls, this value is just for testing
    public int testPlayerStat = 5;

    int maxRemoveWord = 3;
    int maxStat = 5;

    List<RectTransform> activeCards;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        AmountOfPointsText.text = currentAmountOfPoints.ToString();
        questionsForBoat = Resources.LoadAll<BaseScriptable>("FaseBoatQuestions");
        questionsForAmerica = Resources.LoadAll<BaseScriptable>("FaseAmericaQuestions");
        activeCards = new List<RectTransform>();
        NewQuestion();
    }

    //read scriptable > create new UI element > create cards.

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

    void ReadNewScriptable(BaseScriptable[] chosenFase)
    {
        int randomNumber = UnityEngine.Random.Range(0, chosenFase.Length);
        currentQuestion = chosenFase[randomNumber];
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
            newCard.SetParent(answerParent);
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
            AddPoints();
        }
        else
        {
            RemovePoints();
        }
        SetTextToCanvas(currentAmountOfPoints);
        NewQuestion();
    }

    void AddPoints()
    {
        currentAmountOfPoints++;
    }
    void RemovePoints()
    {
        currentAmountOfPoints--;
    }
    void EmptyActiveList()
    {
        foreach (RectTransform benjaminNetanyahu in activeCards)
        {
            Destroy(benjaminNetanyahu.gameObject);
        }
        activeCards.Clear();
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
            int currentRandom = Random.Range(1, words.Length);
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
}
