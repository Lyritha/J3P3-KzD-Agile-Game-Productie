using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    [SerializeField]
    private AudioSource loopAudioSourcePrefab;


    [SerializeField]
    private AudioSource singleShotSource;
    [SerializeField] 
    private AudioSource musicAudioSource;
    private readonly Dictionary<AudioClip, AudioSource> loopingAudioSources = new();


    [SerializeField]
    private LoopedSoundEffect[] loopedSoundEffects;

    [SerializeField]
    private SoundEffect[] soundEffects;

    [SerializeField]
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

    public void AddLoopingSound(LoopedSoundEffects effect)
    {
        foreach (LoopedSoundEffect sound in loopedSoundEffects)
        {
            if (sound.effect == effect)
            {
                // do not play the same clip twice
                if (loopingAudioSources.ContainsKey(sound.clip)) return;

                AudioSource audioSource = Instantiate(loopAudioSourcePrefab, transform);
                audioSource.gameObject.name = $"looped player: {sound.clip.name}";
                audioSource.clip = sound.clip;
                audioSource.loop = true;
                audioSource.volume = sound.volume;

                loopingAudioSources.Add(sound.clip, audioSource);
                audioSource.Play();
                return;
            }
        }

        Debug.LogWarning($"Looped sound not found: {effect}");
    }

    public void StopAllLoopingSounds()
    {
        foreach (AudioSource audioSource in loopingAudioSources.Values)
        {
            if (audioSource != null)
            {
                audioSource.Stop();
                Destroy(audioSource.gameObject);
            }
        }

        loopingAudioSources.Clear();
    }

    public void StopLoopingSound(LoopedSoundEffects effect)
    {
        foreach (LoopedSoundEffect sound in loopedSoundEffects)
        {
            if (sound.effect == effect)
            {
                if (sound.clip == null) return;

                if (loopingAudioSources.TryGetValue(sound.clip, out AudioSource audioSource))
                {
                    audioSource.Stop();
                    Destroy(audioSource.gameObject);
                    loopingAudioSources.Remove(sound.clip);
                }
                return;
            }
        }

        Debug.LogWarning($"Looped sound not found: {effect}");
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

[Serializable]
public struct LoopedSoundEffect
{
    public AudioClip clip;
    public float volume;
    public LoopedSoundEffects effect;
}

public enum SoundEffects
{
    button
}

public enum LoopedSoundEffects
{
    Achterhoek,
    Pheonix,
    Amerika,
    Minigame,
}