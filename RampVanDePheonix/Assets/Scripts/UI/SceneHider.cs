using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SceneHider : MonoBehaviour
{
    [SerializeField]
    private List<CanvasGroup> elementsToHide = new List<CanvasGroup>();
    [SerializeField]
    private List<MonoBehaviour> monoBehavioursToDisable = new List<MonoBehaviour>();
    private List<bool> originalMonoStates = new List<bool>();

    private void Start()
    {
        originalMonoStates = new List<bool>(monoBehavioursToDisable.Count);
        // Fill the list with false (or initial enabled states)
        for (int i = 0; i < monoBehavioursToDisable.Count; i++)
        {
            originalMonoStates.Add(monoBehavioursToDisable[i].enabled);
        }
    }

    public void HideMainScene()
    {
        foreach (CanvasGroup element in elementsToHide)
        {
            element.alpha = 0f;
            element.blocksRaycasts = false;
            element.interactable = false;
        }

        for (int i = 0; i < monoBehavioursToDisable.Count; i++)
        {
            originalMonoStates[i] = monoBehavioursToDisable[i].enabled;
            monoBehavioursToDisable[i].enabled = false;
        }
    }

    public void ShowMainScene()
    {
        foreach (CanvasGroup element in elementsToHide)
        {
            element.alpha = 1f;
            element.blocksRaycasts = true;
            element.interactable = true;
        }

        for (int i = 0; i < monoBehavioursToDisable.Count; i++)
        {
            monoBehavioursToDisable[i].enabled = originalMonoStates[i];
        }
    }
}
