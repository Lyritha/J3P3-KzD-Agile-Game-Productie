using System.Collections.Generic;
using UnityEngine;

public class PartyGen : MonoBehaviour
{
    public List<Personage> party = new List<Personage>();
    void Start()
    {
        Personage[] personages = Resources.LoadAll<Personage>("Personages");

        do
        {
            int randomNum = Random.Range(0, personages.Length);
            if (!party.Contains(personages[randomNum])) party.Add(Instantiate(personages[randomNum]));
            //if (!party.Contains(personages[randomNum])) party.Add(new Personage(personages[randomNum].id, personages[randomNum].name, personages[randomNum].baseKapitaal, personages[randomNum].baseBouwkunde, personages[randomNum].baseLeervermogen, personages[randomNum].baseSociaal, personages[randomNum].baseAanpassingsvermogen, personages[randomNum].woonplaats, personages[randomNum].beroep, personages[randomNum].loreDrop, personages[randomNum].portrait));

        } while (party.Count < 4);

        foreach(Personage personage in party)
        {
            personage.characterName = "patat";
            print(personage.characterName);
        }
    }
}
