using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PartyGen : MonoBehaviour
{
    CharacterListDisplay characterList;
    Personage[] personages;
    public List<Personage> party = new List<Personage>();
    void Start()
    {
        personages = Resources.LoadAll<Personage>("Personages");
        characterList = FindAnyObjectByType<CharacterListDisplay>();

        do
        {
            int randomNum = Random.Range(0, personages.Length);
            if (!CompareCharacter(randomNum)) party.Add(Instantiate(personages[randomNum]));
            else continue;
            //if (!party.Contains(personages[randomNum])) party.Add(new Personage(personages[randomNum].id, personages[randomNum].name, personages[randomNum].baseKapitaal, personages[randomNum].baseBouwkunde, personages[randomNum].baseLeervermogen, personages[randomNum].baseSociaal, personages[randomNum].baseAanpassingsvermogen, personages[randomNum].woonplaats, personages[randomNum].beroep, personages[randomNum].loreDrop, personages[randomNum].portrait));

        } while (party.Count < 4);

        characterList.AddCharacters(party);
    }


    bool CompareCharacter(int randomNum)
    {
        foreach(Personage personage in party)
        {
            if(personage.id == personages[randomNum].id) return true;
        }
        return false;
    }
}
