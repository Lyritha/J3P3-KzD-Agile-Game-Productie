using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

[CreateAssetMenu(fileName = "Event", menuName = "Events/Event")]


public class Event : ScriptableObject
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
    [TextArea]
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
    HungerPerPerson,
    Death,
    Reddingsboot
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