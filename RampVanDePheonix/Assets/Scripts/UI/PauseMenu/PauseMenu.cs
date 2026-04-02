using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    protected CanvasGroup pauseUI;

    [SerializeField]
    private Slider musicVolume;
    [SerializeField]
    private Slider effectsVolume;
    [SerializeField]
    private Slider ambianceVolume;

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

        if (mixer.GetFloat("AmbianceVolume", out float dbAmbiance))
        {
            float linear = Mathf.Pow(10f, dbAmbiance / 20f);
            ambianceVolume.value = linear;
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

    public void SetAmbianceVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        float dB = Mathf.Log10(volume) * 20;
        mixer.SetFloat("AmbianceVolume", dB);
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public virtual void OpenMenu()
    {
    }

    public virtual void CloseMenu()
    {
    }


    protected void SetCanvasGroup(CanvasGroup group ,bool state)
    {
        if (group == null) return;

        group.alpha = state ? 1 : 0;
        group.interactable = state;
        group.blocksRaycasts = state;
    }
}
