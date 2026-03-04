using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class LifeboatManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Character> LifeBoatPassengers;
    //[SerializeField] gameobkj3t BoatSeatsPlacement;
    GameObject Boat;
    void Start()
    {
        Boat = gameObject;
        LifeBoatPassengers = new List<Character>();
    }


    //use this method to save the currently selected character (selected from another script)
    //probably has some bugs no cap
    [ContextMenu("save")]
    public void SaveSelected()
    {
        if (CharacterListDisplay.Instance.TryGetSelectedCharacter(out Character character))
        {
            ChooseCharacterToSafe(character);
        }
    }

    void ChooseCharacterToSafe(Character character)
    {
        //add character to list
        LifeBoatPassengers.Add(character);
        //set status of character to safed
        character.Safe("has been a good boy");
    }
}
