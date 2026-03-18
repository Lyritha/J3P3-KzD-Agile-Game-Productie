using UnityEngine;
using UnityEngine.UI;

//this script is used to initialize the learning minigame 
public class InitializeMinigame : MonoBehaviour
{
    //the list of different englishman onboard the phoenix
    [SerializeField] Sprite[] differentEnglishMen;
    [SerializeField] GameObject[] SceneryObjects;
    //0=achterhoek, 1=boat 2=america

    GameObject player;
    GameObject englishMan;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = transform.GetChild(0).gameObject;
        englishMan = transform.GetChild(1).gameObject;
        SpawnInScenery(EventStateManager.Instance.FaseManager.CurrentFase);
        ChangeDefaultPlayerSprite(player);
        ChangeDefaultEnglishManSprite(englishMan);
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

    void SpawnInScenery(Fases currentFase)
    {
        switch (currentFase)
        {
            case Fases.Achterhoek:
                InstantiateCorrectScenery(SceneryObjects[0]);
                break;
            case Fases.Pheonix:
                InstantiateCorrectScenery(SceneryObjects[1]);
                break;
            case Fases.Amerika:
                InstantiateCorrectScenery(SceneryObjects[2]);
                break;
            default:
                print("COULDNT FIND CORRECT FASE, DEFAULTING RN");
                InstantiateCorrectScenery(SceneryObjects[0]);
                break;
        }
    }
    void InstantiateCorrectScenery(GameObject correctSceneryObject)
    {
        Vector3 sceneryObjectsPosition = new Vector3(0, 0, 0);
        GameObject newGameObject = Instantiate(correctSceneryObject);
        newGameObject.transform.position = sceneryObjectsPosition;
    }



}
