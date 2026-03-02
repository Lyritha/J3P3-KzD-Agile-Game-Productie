using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameQuestion", menuName = "QuestionMinigameScriptable")]

public class BaseScriptable : ScriptableObject
{
    public string AmericanManSays;
    public answer[] allAnswers;
}

[Serializable]
public struct answer
{
    public string anAnswer;
    public bool isCorrect;
}
