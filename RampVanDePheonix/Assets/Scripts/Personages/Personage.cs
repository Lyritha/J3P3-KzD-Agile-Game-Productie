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



    public Personage(int _id, string _name, int _baseKapitaal, int _baseBouwkunde, int _baseLeervermogen, int _baseSociaal, int _baseAanpassingsvermogen, string _woonplaats, string _beroep, string _loreDrop, Sprite _portrait)
    {
        _id = id;
        _name = characterName;
        _baseKapitaal = baseKapitaal;
        _baseBouwkunde = baseBouwkunde;
        _baseLeervermogen = baseLeervermogen;
        _baseSociaal = baseSociaal;
        _baseAanpassingsvermogen = baseAanpassingsvermogen;
        _woonplaats = woonplaats;
        _beroep = beroep;
        _loreDrop = loreDrop;
        _portrait = portrait;
    }
}
