using UnityEngine;
using UnityEngine.UI;

//this script is used to initialize the learning minigame 
public class InitializeMinigame : MonoBehaviour
{
    //the list of different englishman onboard the phoenix
    [SerializeField] Sprite[] differentEnglishMen;

    GameObject player;
    GameObject englishMan;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = transform.GetChild(0).gameObject;
        englishMan = transform.GetChild(1).gameObject;

        ChangeDefaultPlayerSprite(player);
        ChangeDefaultEnglishManSprite(englishMan);
        //KillMySelf();
    }
    /// <summary>
    /// changes the defaultPlayersprite in the scene
    /// </summary>
    void ChangeDefaultPlayerSprite(GameObject player)
    {
        Sprite selected = CharacterListDisplay.Instance.SelectedCharacter.Personage.portrait;
        player.GetComponent<Image>().sprite = selected;
    }

    /// <summary>
    /// changes the sprite from the default englishman in the scene
    /// </summary>
    void ChangeDefaultEnglishManSprite(GameObject englishMan)
    {
        englishMan.GetComponent<Image>().sprite = ChooseRandomPersonSprite(differentEnglishMen);
    }
    /// <summary>
    /// chooses a random sprite out of an array of sprites
    /// </summary>
    Sprite ChooseRandomPersonSprite(Sprite[] personList)
    {
        int randomNum = Random.Range(0, personList.Length);
        return personList[randomNum];
    }

    void KillMySelf()
    {
        Destroy(gameObject.GetComponent<InitializeMinigame>());
    }
}
