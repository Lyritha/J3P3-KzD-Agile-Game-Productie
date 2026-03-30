using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private SceneHider hider;

    [SerializeField]
    private CanvasGroup pauseUI;

    [SerializeField]
    private Slider musicVolume;
    [SerializeField]
    private Slider effectsVolume;

    private void Start()
    {
        if (mixer.GetFloat("MusicVolume", out float dB))
        {
            float linear = Mathf.Pow(10f, dB / 20f);
            musicVolume.value = linear;
        }

        if (mixer.GetFloat("EffectsVolume", out float dBEffects))
        {
            float linear = Mathf.Pow(10f, dBEffects / 20f);
            effectsVolume.value = linear;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseUI.alpha > 0) CloseMenu();
            else OpenMenu();
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
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void OpenMenu()
    {
        if (hider.IsHidden) return;

        Time.timeScale = 0f;

        SetCanvasGroup(pauseUI, true);
        hider.HideMainScene(false);
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f;

        SetCanvasGroup(pauseUI, false);
        hider.ShowMainScene();
    }


    private void SetCanvasGroup(CanvasGroup group ,bool state)
    {
        if (group == null) return;

        group.alpha = state ? 1 : 0;
        group.interactable = state;
        group.blocksRaycasts = state;
    }
}
