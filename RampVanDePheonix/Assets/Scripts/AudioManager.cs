using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;


    [SerializeField, Header("Sources")]
    private AudioSource singleShotSource;
    [SerializeField] 
    private AudioSource musicAudioSource;
    [SerializeField]
    private AudioSource ambientAudioSource;

    [SerializeField]
    private SoundEffect[] soundEffects;

    [SerializeField, Header("Ambiance")]
    private AudioClip achterhoekAmbiance;
    [SerializeField]
    private AudioClip boatAmbiance;
    [SerializeField]
    private AudioClip amerikaAmbiance;

    [SerializeField, Header("Music")]
    private AudioClip achterhoekMusic;
    [SerializeField]
    private AudioClip boatMusic;
    [SerializeField]
    private AudioClip amerikaMusic;
    [SerializeField]
    private AudioClip minigameMusic;


    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<AudioManager>();

                if (_instance == null)
                {
                    AudioManager prefab = Resources.Load<AudioManager>("AudioManager");

                    if (prefab == null)
                    {
                        Debug.LogError("AudioManager prefab not found in Resources!");
                        return null;
                    }

                    _instance = Instantiate(prefab);
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingleClip(SoundEffects effect)
    {
        foreach (SoundEffect sound in soundEffects)
        {
            if (sound.effect == effect)
            {
                singleShotSource.PlayOneShot(sound.clip, sound.volume);
                return;
            }
        }

        Debug.LogWarning($"Sound effect not found: {effect}");
    }

    public void SetPhaseAmbiance(Fases fase)
    {
        AudioClip targetAudioclip;

        targetAudioclip = fase switch
        {
            Fases.Achterhoek => achterhoekAmbiance,
            Fases.Pheonix => boatAmbiance,
            Fases.Amerika => amerikaAmbiance,
            _ => null
        };

        if (targetAudioclip != ambientAudioSource.clip)
        {
            ambientAudioSource.Stop();
            ambientAudioSource.loop = true;
            ambientAudioSource.clip = targetAudioclip;
            ambientAudioSource.Play();
        }
    }

    public void StopAmbiance()
    {
        if (ambientAudioSource.isPlaying)
        {
            ambientAudioSource.Stop();
            ambientAudioSource.clip = null;
        }
    }

    public void SetPhaseMusic(Fases fase, bool isMinigame = false)
    {
        AudioClip targetAudioclip;
        if (isMinigame) targetAudioclip = minigameMusic;
        else
        {
            targetAudioclip = fase switch
            {
                Fases.Achterhoek => achterhoekMusic,
                Fases.Pheonix => boatMusic,
                Fases.Amerika => amerikaMusic,
                _ => null
            };
        }

        if (targetAudioclip != musicAudioSource.clip)
        {
            musicAudioSource.Stop();
            musicAudioSource.loop = true;
            musicAudioSource.clip = targetAudioclip;
            musicAudioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
            musicAudioSource.clip = null;
        }
    }
}

[Serializable]
public struct SoundEffect
{
    public AudioClip clip;
    public float volume;
    public SoundEffects effect;
}

public enum SoundEffects
{
    button
}