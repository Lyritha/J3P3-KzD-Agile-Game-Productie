using System.Collections.Generic;
using UnityEngine;

public class PartyGen : MonoBehaviour
{
    List<Personage> party = new List<Personage>();
    void Start()
    {
        Personage[] personages = Resources.LoadAll<Personage>("Personages");

        do
        {
            int randomNum = Random.Range(0, personages.Length);
            if (!party.Contains(personages[randomNum])) party.Add(personages[randomNum]);

        } while (party.Count < 4);

        foreach (Personage personage in party)
        {
            print(personage.name);
        }

    }
}
