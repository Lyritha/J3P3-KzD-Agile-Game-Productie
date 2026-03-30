using MyBox;
using UnityEngine;
using UnityEngine.UI;

//this script is used to initialize the learning minigame 
public class InitializeMinigame : MonoBehaviour
{
    //the list of different englishman onboard the phoenix
    [SerializeField] Sprite[] differentEnglishMen;

    [SerializeField] GameObject[] SceneryObjects;
    [SerializeField] Sprite[] BackgroundImages;
    //0=achterhoek, 1=boat 2=america
    [SerializeField] GameObject DefaultBackground;


    GameObject currentScenery;
    Sprite currentBackground;

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
        if (CharacterListDisplay.Instance == null) return;

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
                InstantiateCorrectBackground(BackgroundImages[0]);
                gameObject.GetComponent<Animator>().enabled = false;
                break;
            case Fases.Pheonix:
                InstantiateCorrectScenery(SceneryObjects[1]);
                InstantiateCorrectBackground(BackgroundImages[1]);
                break;
            case Fases.Amerika:
                InstantiateCorrectScenery(SceneryObjects[2]);
                InstantiateCorrectBackground(BackgroundImages[2]);
                gameObject.GetComponent<Animator>().enabled = false;
                break;
            default:
                print("COULDNT FIND CORRECT FASE, DEFAULTING RN");
                InstantiateCorrectScenery(SceneryObjects[0]);
                InstantiateCorrectBackground(BackgroundImages[0]);
                gameObject.GetComponent<Animator>().enabled = false;
                break;
        }
    }
    void InstantiateCorrectScenery(GameObject correctSceneryObject)
    {
        GameObject newGameObject = Instantiate(correctSceneryObject);
        newGameObject.transform.position = new Vector3(0, 0, -0.2f);
        currentScenery = newGameObject;
    }

    void InstantiateCorrectBackground(Sprite correctBackgroundSprite)
    {
        print("instantiated a background" + correctBackgroundSprite);
        DefaultBackground.GetComponent<SpriteRenderer>().sprite = correctBackgroundSprite;
        currentBackground = correctBackgroundSprite;
    }

    public void RecheckScenery()
    {
        SpawnInScenery(EventStateManager.Instance.FaseManager.CurrentFase);
    }
}
