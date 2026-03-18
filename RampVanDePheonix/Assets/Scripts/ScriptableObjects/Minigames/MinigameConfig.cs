using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigameConfig", menuName = "Scriptable Objects/MinigameConfig")]
public class MinigameConfig : ScriptableObject
{
    public List<Minigame> minigames;
}
