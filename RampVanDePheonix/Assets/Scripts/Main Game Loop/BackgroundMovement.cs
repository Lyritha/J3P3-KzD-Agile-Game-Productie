using MyBox;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float ImageScaling = 1.4f;
    float backgroundImageWidth = 800;
    float foregroundImageWidth = 800;

    float foregroundSpeed = 400;
    float backgroundSpeed = 250;

    [SerializeField]
    private RectTransform backgroundParent;
    [SerializeField] 
    private RectTransform foregroundParent;


    [Header("background sprites")]
    [SerializeField] Sprite[] backgroundAchterhoek;
    [SerializeField] Sprite[] backgroundBoat;
    [SerializeField] Sprite[] backgroundAmerica;

    [Header("foreground sprites")]
    [SerializeField] Sprite[] foregroundAchterhoek;
    [SerializeField] Sprite[] foregroundBoat;
    [SerializeField] Sprite[] foregroundAmerica;

    [Header("in-scene objects")]
    [SerializeField] GameObject defaultImageObject;
    [SerializeField] Canvas activeCanvas;

    List<GameObject> currentActiveBackgrounds;
    List<GameObject> currentActiveForeGrounds;

    int backgroundCount = 0;
    int foregroundCount = 0;

    int amountOfBackgroundSpawns = 0;
    int amountOfForegroundSpawns = 0;

    int imagePixelOverlap = 5;
    public bool isTraveling = true;

    //default positions are based on 1080 x 1920
    [SerializeField] Vector3 backgroundSpawnPosition = new Vector3(-500, 540, 0);
    [SerializeField] Vector3 foregroundSpawnPosition = new Vector3(-500, 270, 0.1f);
    [SerializeField] private float foregroundOffset = 0;


    void Start()
    {
        //scales the spawning position according to the rendering height
        backgroundSpawnPosition.y = 0;
        foregroundSpawnPosition.y = foregroundOffset;



        backgroundSpeed = (foregroundSpeed * 0.5f);

        backgroundImageWidth -= imagePixelOverlap;
        foregroundImageWidth -= imagePixelOverlap;

        currentActiveBackgrounds = new List<GameObject>();
        currentActiveForeGrounds = new List<GameObject>();

        FillScreenOnStart();
        backgroundCount = currentActiveBackgrounds.Count;
    }

    private void FixedUpdate()
    {
        if (isTraveling)
        {
            ManageBackgroundItems();
            ManagerForegroundItems();
        }
    }

    void ManagerForegroundItems()
    {
        if (currentActiveForeGrounds.Count > 0)
        {
            if (currentActiveForeGrounds[foregroundCount - 1].transform.position.x > (foregroundImageWidth + backgroundSpawnPosition.x)-100)
            {
                AddNewForeGround(foregroundSpawnPosition);
                if (currentActiveForeGrounds.Count > 5)
                {
                    RemoveLastForeground();
                }
            }
            MoveAllActiveForegroundItems(foregroundSpeed);
        }
    }

    void ManageBackgroundItems()
    {
        if (currentActiveBackgrounds.Count > 0)
        {
            if (currentActiveBackgrounds[backgroundCount - 1].transform.position.x > (backgroundImageWidth + backgroundSpawnPosition.x))
            {
                AddNewBackground(backgroundSpawnPosition);
                if (backgroundCount > 5)
                {
                    RemoveLastBackground();
                }
            }
            MoveAllActiveBackgroundItems(backgroundSpeed);
        }
    }

    Sprite SelectBackgroundAccordingToFase(Fases currentFase)
    {
        Sprite background = null;
        switch (currentFase)
        {
            case Fases.Achterhoek:
                background = RandomSprite(backgroundAchterhoek);
                break;
            case Fases.Pheonix:
                background = RandomSprite(backgroundBoat);
                break;
            case Fases.Amerika:
                background = RandomSprite(backgroundAmerica);
                break;
            default:
                background = RandomSprite(backgroundAchterhoek);
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
                foreground = RandomSprite(foregroundAchterhoek);
                break;
            case Fases.Pheonix:
                foreground = RandomSprite(foregroundBoat);
                break;
            case Fases.Amerika:
                foreground = RandomSprite(foregroundAmerica);
                break;
            default:
                foreground = RandomSprite(foregroundAchterhoek);
                break;
        }
        return foreground;
    }

    void AddNewBackground(Vector3 position)
    {
        Sprite newBackgroundElement = SelectBackgroundAccordingToFase(Fases.Achterhoek); //// zet hier nog de reference naar fasemanager heen
        GameObject newbackground = Instantiate(defaultImageObject, backgroundParent);
        newbackground.GetComponent<Image>().sprite = newBackgroundElement;

        if (amountOfBackgroundSpawns % 2 == 0)
        {
            position.z += 0.20f;
        }
        newbackground.transform.position = position;
        currentActiveBackgrounds.Add(newbackground);
        backgroundCount = currentActiveBackgrounds.Count;
        amountOfBackgroundSpawns++;
    }

    void AddNewForeGround(Vector3 position)
    {
        Sprite newForeGroundSprite = SelectForegroundAccordingToFase(Fases.Achterhoek); //// zet hier nog de reference naar fasemanager heen
        GameObject newForeGround = Instantiate(defaultImageObject, foregroundParent);
        newForeGround.GetComponent<Image>().sprite = newForeGroundSprite;

        //layers the sprite to prevent z fighting
        if (amountOfForegroundSpawns % 2 == 0)
        {
            position.z += 0.05f;
        }
        newForeGround.transform.position = position;
        currentActiveForeGrounds.Add(newForeGround);
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

    void RemoveLastForeground()
    {
        GameObject gameObjectToDestroy = currentActiveForeGrounds[0];
        currentActiveForeGrounds.RemoveAt(0);
        Destroy(gameObjectToDestroy);
        foregroundCount = currentActiveForeGrounds.Count;
    }


    Sprite RandomSprite(Sprite[] spriteArray)
    {
        int random = Random.Range(0, spriteArray.Length);
        return spriteArray[random];
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

    void MoveAllActiveForegroundItems(float movementSpeed)
    {
        float delta = Time.deltaTime;
        foreach (GameObject item in currentActiveForeGrounds)
        {
            float actualDistance = movementSpeed * delta;
            item.transform.position += new Vector3(actualDistance, 0, 0);
        }
    }


    /// <summary>
    /// this method fills the screen with images on start 
    /// NOTE: images spawned in this method are from RIGHT to LEFT due to improper removal after an image gets offscreen
    /// </summary>
    void FillScreenOnStart()
    {
        //placements is from right to left otherwise images get removed incorrectly (0 is not the most right one)
        float currentBackgroundPos = 2000;
        float currentForegroundPos = 2000;

        Vector3 backStartPos = backgroundSpawnPosition;
        Vector3 foreStartPos = foregroundSpawnPosition;

        //had to do it this way because unity start function is retarded 
        for (int i = 0; i < 4; i++)
        {
            Vector3 tempVector = new Vector3(currentBackgroundPos, backStartPos.y, backStartPos.z);
            AddNewBackground(tempVector);
            currentBackgroundPos -= (backgroundImageWidth - imagePixelOverlap);
        }

        for (int i = 0; i < 4; i++)
        {
            Vector3 tempVector = new Vector3(currentForegroundPos, foreStartPos.y, foreStartPos.z);
            AddNewForeGround(tempVector);
            currentForegroundPos -= (backgroundImageWidth - imagePixelOverlap);
        }
    }
}
