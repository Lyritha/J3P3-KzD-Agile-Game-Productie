using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float ImageScaling = 1.4f;
    float backgroundImageWidth = 800;
    float foregroundImageWidth = 800;

    float pauseSpeed = 0.5f;
    float targetForegroundSpeed;
    float targetBackgroundSpeed;
    float baseForegroundSpeed = 400;
    float baseBackgroundSpeed = 250;
    float currentForegroundSpeed = 0;
    float currentBackgroundSpeed = 0;

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

    [SerializeField] Boat boat;
    [SerializeField] BrendaFlip brenda;

    [SerializeField] GameObject boatPlayer;
    [SerializeField] GameObject brendaPlayer;


    List<GameObject> currentActiveBackgrounds = new();
    List<GameObject> currentActiveForeGrounds = new();

    int backgroundCount = 0;
    int foregroundCount = 0;

    int amountOfBackgroundSpawns = 0;
    int amountOfForegroundSpawns = 0;

    int imagePixelOverlap = 5;
    public bool isTraveling = true;

    //default positions are based on 1080 x 1920
    [SerializeField] Vector3 backgroundSpawnPosition = new Vector3(-500, 0, 0);
    [SerializeField] Vector3 foregroundSpawnPosition = new Vector3(-500, 0, 0.1f);
    [SerializeField] private float foregroundOffset = 0;


    private Fases currentFase;

    void Start()
    {
        //scales the spawning position according to the rendering height
        backgroundSpawnPosition.y = 0;
        foregroundSpawnPosition.y = foregroundOffset;

        currentBackgroundSpeed = (currentForegroundSpeed * 0.5f);

        backgroundImageWidth -= imagePixelOverlap;
        foregroundImageWidth -= imagePixelOverlap;

        currentActiveBackgrounds = new List<GameObject>();
        currentActiveForeGrounds = new List<GameObject>();

        FillScreenOnStart();
        backgroundCount = currentActiveBackgrounds.Count;
    }

    [ContextMenu("pause")]
    public void PauseBackground()
    {
        targetForegroundSpeed = 0;
        targetBackgroundSpeed = 0;
        if(brenda != null) brenda.StopMoving();
        if(boat != null) boat.StopMoving();
    }

    [ContextMenu("start")]
    public void StartBackground()
    {
        targetForegroundSpeed = baseForegroundSpeed;
        targetBackgroundSpeed = baseBackgroundSpeed;
        if(brenda != null) brenda.StartMoving();
        if (boat != null) boat.StartMoving();
    }

    private void Update()
    {
        currentForegroundSpeed = Mathf.MoveTowards(
            currentForegroundSpeed,
            targetForegroundSpeed,
            pauseSpeed * Time.deltaTime * baseForegroundSpeed
        );

        currentBackgroundSpeed = Mathf.MoveTowards(
            currentBackgroundSpeed,
            targetBackgroundSpeed,
            pauseSpeed * Time.deltaTime * baseBackgroundSpeed
        );
    }

    private void FixedUpdate()
    {
        if (isTraveling)
        {
            ManageBackgroundItems();
            ManagerForegroundItems();
        }
    }

    public void SetFase(Fases fase)
    {
        currentFase = fase;
        if (currentFase == Fases.Pheonix)
        {
            boatPlayer.SetActive(true);
            brendaPlayer.SetActive(false);
        }
        else
        {
            boatPlayer.SetActive(false);
            brendaPlayer.SetActive(true);
        }
        FillScreenOnStart();
    }

    void ManagerForegroundItems()
    {
        if (currentActiveForeGrounds.Count == 0) return;

        GameObject newestForeground = currentActiveForeGrounds[^1];
        RectTransform newestRect = newestForeground.GetComponent<RectTransform>();

        // Check if the newest background has entered the visible area
        if (newestRect.anchoredPosition.x > 0)
        {
            // Attach the next background to the right of the newest one
            // Move new background to the left of the newest one (or right depending on movement direction)
            Vector3 newPos = newestRect.anchoredPosition;
            newPos.x -= newestRect.rect.width;

            AddNewForeGround(newPos);

            if (currentActiveForeGrounds.Count > 5)
            {
                RemoveLastForeground();
            }
        }

        MoveAllActiveForegroundItems(currentForegroundSpeed);
    }

    void ManageBackgroundItems()
    {
        if (currentActiveBackgrounds.Count == 0) return;

        GameObject newestBackground = currentActiveBackgrounds[^1];
        RectTransform newestRect = newestBackground.GetComponent<RectTransform>();

        // Check if the newest background has entered the visible area
        if (newestRect.anchoredPosition.x > 0)
        {
            // Attach the next background to the right of the newest one
            // Move new background to the left of the newest one (or right depending on movement direction)
            Vector3 newPos = newestRect.anchoredPosition;
            newPos.x -= newestRect.rect.width;

            AddNewBackground(newPos);

            // Keep pool size limited
            if (backgroundCount > 5)
            {
                RemoveLastBackground();
            }
        }

        MoveAllActiveBackgroundItems(currentBackgroundSpeed);
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
        Sprite newBackgroundElement = SelectBackgroundAccordingToFase(currentFase); //// zet hier nog de reference naar fasemanager heen
        GameObject newbackground = Instantiate(defaultImageObject, backgroundParent);
        newbackground.GetComponent<Image>().sprite = newBackgroundElement;

        if (amountOfBackgroundSpawns % 2 == 0)
        {
            position.z += 0.20f;
        }

        RectTransform rectTransform = newbackground.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        currentActiveBackgrounds.Add(newbackground);
        backgroundCount = currentActiveBackgrounds.Count;
        amountOfBackgroundSpawns++;
    }

    void AddNewForeGround(Vector3 position)
    {
        Sprite newForeGroundSprite = SelectForegroundAccordingToFase(currentFase); //// zet hier nog de reference naar fasemanager heen
        GameObject newForeGround = Instantiate(defaultImageObject, foregroundParent);
        newForeGround.GetComponent<Image>().sprite = newForeGroundSprite;

        //layers the sprite to prevent z fighting
        if (amountOfForegroundSpawns % 2 == 0)
        {
            position.z += 0.05f;
        }

        RectTransform rectTransform = newForeGround.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
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
        float delta = Time.fixedDeltaTime;
        foreach (GameObject item in currentActiveBackgrounds)
        {
            float actualDistance = movementSpeed * delta;
            item.transform.position += new Vector3(actualDistance, 0, 0);
        }
    }

    void MoveAllActiveForegroundItems(float movementSpeed)
    {
        float delta = Time.fixedDeltaTime;
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
        foreach (GameObject obj in currentActiveBackgrounds)
            Destroy(obj);

        foreach (GameObject obj in currentActiveForeGrounds)
            Destroy(obj);

        currentActiveBackgrounds.Clear();
        currentActiveForeGrounds.Clear();

        backgroundCount = 0;
        foregroundCount = 0;
        amountOfBackgroundSpawns = 0;
        amountOfForegroundSpawns = 0;



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
