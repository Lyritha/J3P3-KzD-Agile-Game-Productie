using UnityEngine;

[CreateAssetMenu(fileName = "Personage", menuName = "Scriptable Objects/Personages")]
public class Personage : ScriptableObject
{
    [Header("ID")]
    public int id;

    [Header("Name")]
    public string characterName;

    [Header("Vaardigheden")]
    public int baseKapitaal;
    public int baseBouwkunde;
    public int baseLeervermogen;
    public int baseSociaal;
    public int baseAanpassingsvermogen;

    [Header("Lore")]
    public string woonplaats;
    public string beroep;
    public string loreDrop;

    [Header("Sprite")]
    public Sprite portrait;
}
