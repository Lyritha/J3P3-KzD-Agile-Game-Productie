using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SceneHider : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup foreground;
    [SerializeField] 
    private CanvasGroup background;
    [SerializeField]
    private CanvasGroup minigame;

    [SerializeField]
    private List<MonoBehaviour> foregroundMonobehaviors = new List<MonoBehaviour>();
    private List<bool> originalMonoStates = new List<bool>();

    public bool IsHidden { get; private set; } = false;

    private void Start()
    {
        originalMonoStates = new List<bool>(foregroundMonobehaviors.Count);
        // Fill the list with false (or initial enabled states)
        for (int i = 0; i < foregroundMonobehaviors.Count; i++)
        {
            originalMonoStates.Add(foregroundMonobehaviors[i].enabled);
        }
    }

    public void HideMainScene(bool hideBackground = true)
    {
        SetCanvasGroup(foreground, false);
        if (hideBackground) SetCanvasGroup(background, false);
        SetCanvasGroup(minigame, false);

        for (int i = 0; i < foregroundMonobehaviors.Count; i++)
        {
            originalMonoStates[i] = foregroundMonobehaviors[i].enabled;
            foregroundMonobehaviors[i].enabled = false;
        }

        IsHidden = true;
    }

    public void ShowMainScene()
    {
        SetCanvasGroup(foreground, true);
        SetCanvasGroup(background, true);
        SetCanvasGroup(minigame, true);

        for (int i = 0; i < foregroundMonobehaviors.Count; i++)
        {
            foregroundMonobehaviors[i].enabled = originalMonoStates[i];
        }

        IsHidden = false;
    }

    private void SetCanvasGroup(CanvasGroup group, bool state)
    {
        if (group == null) return;

        group.alpha = state ? 1 : 0;
        group.interactable = state;
        group.blocksRaycasts = state;
    }
}
