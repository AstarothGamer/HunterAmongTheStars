using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SoundType
{
    RifleShot,
    ShotgunShot,
    PistolShot,
    Reloading,
    Swing,
    Hit,
    Damage,
    Death,
    Button,
    Button2,
    Switch,
    Item,
    FootStep,
    Spaceship,
    SpaceshipLightShot,
    SpaceshipHeavyShot,
    SpaceshipLightReloading,
    Boost,
    Money,
    CalmMusic,
    BattleMusic
}
public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    public AudioSource audioSource;
    public AudioSource MusicAudioSource;// for playing music
    public AudioSource LoopAudioSource;// for looping sounds
                                       
    private static AudioManager _instance;

    #region Singleton
    public static AudioManager Instance
    {
        get
        {
            // Check if the instance is already created
            if (_instance == null)
            {
                // Try to find an existing AudioManager in the scene
                _instance = FindAnyObjectByType<AudioManager>();

                // If no AudioManager exists, create a new one
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("AudioManager");
                    _instance = singletonObject.AddComponent<AudioManager>();
                }

                // Make the AudioManager persist across scenes (optional)
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        // If the instance is already set, destroy this duplicate object
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;  // Assign this object as the instance
        }
    }
    #endregion

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        if (Instance.soundList == null || Instance.soundList.Length <= (int)sound)
        {
            Debug.LogWarning("Sound list is not properly assigned or the sound index is out of range.");
            return;
        }

        AudioClip clip = Instance.soundList[(int)sound].Sound;
        if (clip != null)
        {
            if (Instance.audioSource != null)
            {
                Instance.audioSource.PlayOneShot(clip, volume);
            }
            else
            {
                Debug.LogWarning("Audio source is not assigned in the AudioManager.");
            }
        }
        else
        {
            Debug.LogWarning($"Sound not found for: {sound}");
        }
    }
    public static void PlaySoundAtPoint(SoundType sound, Vector3 pos, float volume = 1)
    {
        AudioClip clip = Instance.soundList[(int)sound].Sound;
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, volume);
        }
        else
        {
            Debug.LogWarning($"Sound not found for: {sound}");
        }
    }
    public static void PlayMusic(SoundType sound, float volume = 1f)
    {
        AudioClip clip = Instance.soundList[(int)sound].Sound;
        if (clip != null)
        {
            Instance.MusicAudioSource.clip = clip; // Assign the clip to the audio source
            Instance.MusicAudioSource.volume = volume; // Set the volume
            Instance.MusicAudioSource.loop = true; // Enable looping
            Instance.MusicAudioSource.Play(); // Play the clip
        }
        else
        {
            Debug.LogWarning($"Music not found for: {sound}");
        }
    }
    public static void StopMusicGradually(float fadeDuration = 2f)
    {
        Instance.StartCoroutine(FadeOutMusic(fadeDuration));
    }
    public static IEnumerator FadeOutMusic(float duration)
    {
        AudioSource audioSource = Instance.MusicAudioSource;
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = startVolume; // Resets the volume
    }
    #region Loop sound
    public static void PlayLoopSound(SoundType sound, float volume = 1f)
    {
        AudioClip clip = Instance.soundList[(int)sound].Sound;
        if (clip != null)
        {
            Instance.MusicAudioSource.clip = clip; // Assign the clip to the audio source
            Instance.MusicAudioSource.volume = volume; // Set the volume
            Instance.MusicAudioSource.loop = true; // Enable looping
            Instance.MusicAudioSource.Play(); // Play the clip
        }
        else
        {
            Debug.LogWarning($"Music not found for: {sound}");
        }
    }
    public static void StopLoopSoundGradually(float fadeDuration = 2f)
    {
        Instance.StartCoroutine(FadeOutLoopSound(fadeDuration));
    }
    public static IEnumerator FadeOutLoopSound(float duration)
    {
        AudioSource loopAudioSource = Instance.MusicAudioSource;
        float startVolume = loopAudioSource.volume;

        while (loopAudioSource.volume > 0)
        {
            loopAudioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }
        loopAudioSource.Stop();
        loopAudioSource.volume = startVolume; // Resets the volume
    }
    #endregion
}
[Serializable]
public struct SoundList
{
    [SerializeField] private string name;
    [SerializeField] public AudioClip Sound;
}