using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AanpassingControlls : MonoBehaviour
{
    [SerializeField] Image cartImage;
    [SerializeField] TMP_Text progressText;
    [SerializeField] TMP_Text stateText;
    [SerializeField] float drainSpeed = 2f;

    private bool backNForth = false;

    private float progressCounter;

    void Start()
    {
        progressCounter = 0f;
        backNForth = false;
    }

    void Update()
    {

        if (progressCounter >= 90f)
        {
            cartImage.rectTransform.anchoredPosition = new Vector2(-425, -10);
            cartImage.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            stateText.text = "Los";
        }

        else if (progressCounter >= 60f)
        {
            cartImage.rectTransform.anchoredPosition = new Vector2(-425, -150);
            cartImage.rectTransform.rotation = Quaternion.Euler(0, 0, -10);
            stateText.text = "Deels los";
        }

        else if (progressCounter >= 30f)
        {
            cartImage.rectTransform.anchoredPosition = new Vector2(-425, -200);
            cartImage.rectTransform.rotation = Quaternion.Euler(0, 0, -15);
     
            stateText.text = "Deels vast";
        }

        else if (progressCounter >= 0f)
        {
            cartImage.rectTransform.anchoredPosition = new Vector2(-425, -250);
            cartImage.rectTransform.rotation = Quaternion.Euler(0, 0, -20);
            stateText.text = "Volledig vast";
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!backNForth)
            {
                progressCounter -= 10;
            }
            else
            {
                progressCounter += 1;
                backNForth = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (backNForth)
            {
                progressCounter -= 10;
            }
            else
            {
                progressCounter += 1;
                backNForth = true;
            }
        }

        progressCounter = Mathf.Clamp(progressCounter, 0f, 100f);

        if (progressCounter > 0f)
        {
            progressCounter -= drainSpeed * Time.deltaTime;
            progressCounter = Mathf.Max(progressCounter, 0f);
        }

        progressText.text = Mathf.RoundToInt(progressCounter) + "%";
    }
}