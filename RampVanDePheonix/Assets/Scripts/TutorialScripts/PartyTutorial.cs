using System.Collections.Generic;
using UnityEngine;

public class PartyTutorial : MonoBehaviour
{
    CharacterListDisplay characterList;

    [SerializeField]
    private List<Personage> partyData = new();
    public List<Personage> party = new();

    void Start()
    {
        characterList = FindAnyObjectByType<CharacterListDisplay>();

        for (int i = 0; i < partyData.Count; i++)
        {
            party.Add(Instantiate(partyData[i]));
        }

        characterList.AddCharacters(party);
    }
}
