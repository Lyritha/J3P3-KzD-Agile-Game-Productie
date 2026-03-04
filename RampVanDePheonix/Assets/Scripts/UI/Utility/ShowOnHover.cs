using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowOnHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{

    [SerializeField] 
    private float fadeSpeed = 5f;
    [SerializeField]
    private CanvasGroup showOnHoverElement;

    private Coroutine fadeRoutine;
    private bool isEnabled = true;

    private void Awake() => Hide();

    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
        if (!isEnabled) Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isEnabled) Show();
    }

    public void OnPointerExit(PointerEventData eventData) => Hide();


    private void Show() => StartFade(1f);
    private void Hide() => StartFade(0f);


    private void StartFade(float target)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(target));
    }

    private IEnumerator Fade(float target)
    {
        while (!Mathf.Approximately(showOnHoverElement.alpha, target))
        {
            showOnHoverElement.alpha = Mathf.MoveTowards(showOnHoverElement.alpha, target, fadeSpeed * Time.deltaTime);
            yield return null;
        }

        showOnHoverElement.interactable = target == 1f;
        showOnHoverElement.blocksRaycasts = target == 1f;
    }
}
