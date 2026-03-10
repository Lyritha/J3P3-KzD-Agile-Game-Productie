using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private CanvasGroup pauseUI;
    [SerializeField]
    private CanvasGroup uiToHide;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseUI.alpha > 0)
                CloseMenu();
            else
                OpenMenu();
        }
    }

    public void SetMusicVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        float dB = Mathf.Log10(volume) * 20;
        mixer.SetFloat("MusicVolume", dB);
    }

    public void SetEffectVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        float dB = Mathf.Log10(volume) * 20;
        mixer.SetFloat("EffectsVolume", dB);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void OpenMenu()
    {
        Time.timeScale = 0f; // Pause the game

        SetCanvasGroup(pauseUI, true);
        SetCanvasGroup(uiToHide, false);
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f; // Resume the game

        SetCanvasGroup(pauseUI, false);
        SetCanvasGroup(uiToHide, true);
    }


    private void SetCanvasGroup(CanvasGroup group ,bool state)
    {
        if (group == null) return;

        group.alpha = state ? 1 : 0;
        group.interactable = state;
        group.blocksRaycasts = state;
    }
}
