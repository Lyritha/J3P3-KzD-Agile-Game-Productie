using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableQuestion", menuName = "scriptableQuestiom")]


public class QuestionScriptable : ScriptableObject
{
    [TextArea]
 
    public string question;
    public Answer[] answers;
}

[Serializable]
public struct Answer
{
    public SkillNeededForAnswer[] skillNeeded;
    //an action that the player can choose
    public string action;
    //the result of the chosen action
    [TextArea]
    public string result;
    [TextArea]
    public string resultFailed;
    public SkillsetChange[] change;
    public SkillsetChange[] changeFailed;
}


public enum Skillset
{
    AanpassingsVermogen,
    Kapitaal,
    Bouwkunde,
    Socialiteit,
    Leervermogen,
    FoodStorage,
    HungerPerPerson
}
[Serializable]
public struct SkillsetChange
{
    public Skillset skillToBeChanged;
    public int changeAmount;
}

[Serializable]
public struct SkillNeededForAnswer
{
    public Skillset skillType;
    public int skillAmountNeeded;
}