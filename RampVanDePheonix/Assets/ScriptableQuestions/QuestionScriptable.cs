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
    //an action that the player can choose
    public string action;
    //the result of the chosen action
    [TextArea]
    public string result;
    public SkillsetChange[] change;
}


public enum Skillset
{
    AanpassingsVermogen,
    Kapitaal,
    Bouwkunde,
    Socialiteit,
    Leervermogen
}
[Serializable]
public struct SkillsetChange
{
    public Skillset skillToBeChanged;
    public int changeAmount;
}