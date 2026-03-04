using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LifeboatManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    List<GameObject> LifeBoatPassengers;
    [SerializeField] GameObject BoatSeatsPlacement;
    GameObject Boat;
    void Start()
    {
        Boat = gameObject;
        LifeBoatPassengers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChoosePersonToBeSaved(GameObject character)
    {
        LifeBoatPassengers.Add(character);
        SetIsSafeStatus(character);
    }
    void SetIsSafeStatus(GameObject character)
    {
        //get character script and set it to "is safe" (need to change the character script)
    }

    void PlaceCharacterInOpenSeat()
    {

    }
}
