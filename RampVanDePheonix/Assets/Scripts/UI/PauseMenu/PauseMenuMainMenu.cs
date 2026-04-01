using UnityEngine;
using UnityEngine.Audio;

public class PauseMenuMainMenu : PauseMenu
{
    [SerializeField]
    private CanvasGroup otherToHide;

    public override void OpenMenu()
    {
        Time.timeScale = 0f;

        SetCanvasGroup(pauseUI, true);
        SetCanvasGroup(otherToHide, false);
    }

    public override void CloseMenu()
    {
        Time.timeScale = 1f;

        SetCanvasGroup(pauseUI, false);
        SetCanvasGroup(otherToHide, true);
    }
}
