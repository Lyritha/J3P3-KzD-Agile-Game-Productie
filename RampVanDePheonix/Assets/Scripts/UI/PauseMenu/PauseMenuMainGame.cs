using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuMainGame : PauseMenu
{
    [SerializeField]
    private SceneHider hider;

    public override void OpenMenu()
    {
        if(hider != null && hider.IsHidden) return;

        Time.timeScale = 0f;

        SetCanvasGroup(pauseUI, true);
        if (hider != null) hider.HideMainScene(false);
    }

    public override void CloseMenu()
    {
        Time.timeScale = 1f;

        SetCanvasGroup(pauseUI, false);
        if (hider != null) hider.ShowMainScene();
    }
}
