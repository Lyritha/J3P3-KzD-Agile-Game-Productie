using MyBox;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float ImageScaling = 1.4f;
    float backgroundImageWidth = 800;
    float foregroundImageWidth = 800;

    [SerializeField] Sprite[] backgroundAchterhoek;
    [SerializeField] Sprite[] backgroundBoat;
    [SerializeField] Sprite[] backgroundAmerica;
    [SerializeField] GameObject defImageObject;
    [SerializeField] Canvas activeCanvas;
    List<GameObject> currentActiveBackgrounds;
    List<GameObject> currentActiveForeGrounds;

    int backgroundCount = 0;
    int foregroundCount = 0;

    int amountOfBackgroundSpawns = 0;
    int amountOfForegroundSpawns = 0;

    int imageOverlap = 5;
    bool isTraveling = true;
    Vector3 spawnPosition = new Vector3(-400, (1080 / 2), 0);

    void Start()
    {
        backgroundImageWidth -= imageOverlap;
        currentActiveBackgrounds = new List<GameObject>();
        AddNewBackground();
        backgroundCount = currentActiveBackgrounds.Count;
    }

    // Update is called once per frame
    void Update()
    {
        ManageBackgroundItems();

    }


    void ManagerForegroundItems()
    {
        if (currentActiveForeGrounds[foregroundCount-1].transform.position.x > (foregroundImageWidth + spawnPosition.x))
        {
            //AddNewForeground();  >>>>>> rework and check method
            if (currentActiveForeGrounds.Count > 4)
            {
                //removeLastForeground   >>>>>> rework and check method
            }
        }
        //MoveAllActiveForegroundItems(500); >>>>> create method for foreground
    }

    void ManageBackgroundItems()
    {
        if (currentActiveBackgrounds[backgroundCount - 1].transform.position.x > (backgroundImageWidth + spawnPosition.x))
        {
            AddNewBackground();
            if (backgroundCount > 4)
            {
                RemoveLastBackground();
            }
        }
        MoveAllActiveBackgroundItems(500);
    }

    Sprite SelectBackgroundAccordingToFase(Fases currentFase)
    {
        Sprite background = null;
        switch (currentFase)
        {
            case Fases.Achterhoek:
                RandomBackgroundSprite(backgroundAchterhoek);
                break;
            case Fases.Pheonix:
                RandomBackgroundSprite(backgroundBoat);
                break;
            case Fases.Amerika:
                RandomBackgroundSprite(backgroundAmerica);
                break;
            default:
                RandomBackgroundSprite(backgroundAchterhoek);
                break;
        }
        return background;
    }

    Sprite SelectForegroundAccordingToFase(Fases currentFase)
    {
        Sprite foreground = null;
        switch (currentFase)
        {
            case Fases.Achterhoek:
                RandomBackgroundSprite(backgroundAchterhoek);
                break;
            case Fases.Pheonix:
                RandomBackgroundSprite(backgroundBoat);
                break;
            case Fases.Amerika:
                RandomBackgroundSprite(backgroundAmerica);
                break;
            default:
                RandomBackgroundSprite(backgroundAchterhoek);
                break;
        }
        return foreground;
    }

    void AddNewBackground()
    {
        Sprite newBackgroundElement = SelectBackgroundAccordingToFase(Fases.Achterhoek); //// zet hier nog de reference naar fasemanager heen
        GameObject newbackground = Instantiate(defImageObject, activeCanvas.transform);

        if (amountOfBackgroundSpawns % 2 == 0)
        {
            spawnPosition.z = 0.25f;
        }
        else
        {
            spawnPosition.z = 0.00f;
        }
        newbackground.transform.position = spawnPosition;
        currentActiveBackgrounds.Add(newbackground);
        backgroundCount = currentActiveBackgrounds.Count;
        amountOfBackgroundSpawns++;
    }

    void AddNewForeGround()
    {
        Sprite newForeGroundSprite = SelectBackgroundAccordingToFase(Fases.Achterhoek); //// zet hier nog de reference naar fasemanager heen
        GameObject newForeGround = Instantiate(defImageObject, activeCanvas.transform);

        //layers the sprite to prevent z fighting
        if (amountOfForegroundSpawns % 2 == 0)
        {
            spawnPosition.z = 0.25f;
        }
        else
        {
            spawnPosition.z = 0.00f;
        }
        newForeGround.transform.position = spawnPosition;
        currentActiveBackgrounds.Add(newForeGround);
        foregroundCount = currentActiveForeGrounds.Count;
        amountOfForegroundSpawns++;
    }

    void RemoveLastBackground()
    {
        GameObject gameObjectToDestroy = currentActiveBackgrounds[0];
        currentActiveBackgrounds.RemoveAt(0);
        Destroy(gameObjectToDestroy);
        backgroundCount = currentActiveBackgrounds.Count;
    }

    Sprite RandomBackgroundSprite(Sprite[] spriteArray)
    {
        Random.Range(0, spriteArray.Length);
        return spriteArray[spriteArray.Length - 1];
    }

    Sprite RandomForegroundSprite(Sprite[] foregroundSpriteArray)
    {
        Random.Range(0, foregroundSpriteArray.Length);
        return foregroundSpriteArray[foregroundSpriteArray.Length - 1];
    }

    void MoveAllActiveBackgroundItems(float movementSpeed)
    {
        float delta = Time.deltaTime;
        foreach (GameObject item in currentActiveBackgrounds)
        {
            float actualDistance = movementSpeed * delta;
            item.transform.position += new Vector3(actualDistance, 0, 0);
        }
    }
}
