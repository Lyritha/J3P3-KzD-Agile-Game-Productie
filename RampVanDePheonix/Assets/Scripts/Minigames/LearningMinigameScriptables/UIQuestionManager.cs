using MyBox;
using NUnit.Framework;
using System.Collections.Generic;
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
        questionGameObject.GetComponentInChildren<TMP_Text>().text = currentQuestion.AmericanManSays;
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
        AmountOfPointsText.text = currentAmountOfPoints.ToString();
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
}
