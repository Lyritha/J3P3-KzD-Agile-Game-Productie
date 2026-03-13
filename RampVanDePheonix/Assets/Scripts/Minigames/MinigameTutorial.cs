using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MinigameTutorial : MonoBehaviour
{
    [SerializeField]
    private Button lastStepButton;
    [SerializeField]
    private Button nextStepButton;
    [SerializeField]
    private Button minigameMenuButton;
    [SerializeField]
    private RectTransform parentRect;


    private RectTransform[] rectTransforms;

    int currentIndex = 0;
    int maxIndex = 0;

    private void OnEnable()
    {
        if (maxIndex > 0)
        {
            currentIndex = 0;
            SetStep(currentIndex);
        }
    }

    private void Start()
    {
        rectTransforms = new RectTransform[parentRect.childCount];

        for (int i = 0; i < parentRect.childCount; i++)
        {
            rectTransforms[i] = parentRect.GetChild(i).GetComponent<RectTransform>();
        }

        foreach (RectTransform rectTransform in rectTransforms)
        {
            rectTransform.gameObject.SetActive(false);
        }

        maxIndex = rectTransforms.Length - 1;

        // Show first tutorial step
        if (rectTransforms.Length > 0)
            rectTransforms[currentIndex].gameObject.SetActive(true);
    }

    public void NextStep()
    {
        SetStep(currentIndex + 1);
    }

    public void PrevStep()
    {
        SetStep(currentIndex - 1);
    }

    public void SetStep(int index)
    {
        currentIndex = Mathf.Clamp(index, 0, maxIndex);

        for (int i = 0; i < rectTransforms.Length; i++)
        {
            rectTransforms[i].gameObject.SetActive(i == currentIndex);
        }

        // Button states
        lastStepButton.gameObject.SetActive(currentIndex > 0);

        bool isLast = currentIndex == maxIndex;
        nextStepButton.gameObject.SetActive(!isLast);
        minigameMenuButton.gameObject.SetActive(isLast);
    }

    public void CloseTutorial()
    {
        gameObject.SetActive(false);
    }
}
