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
    Switch,
    Item,
    FootStep,
    Spaceship,
    SpaceshipLightShot,
    SpaceshipHeavyShot,
    Boost,
    Money,
    CalmMusic,
    BattleMusic
}
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private SoundList[] soundList;
    public AudioSource audioSource;
    public AudioSource MusicAudioSource;// for playing music
    public AudioSource LoopAudioSource;// for looping sounds
    private void Start()
    {
        //play music on start
    }
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip clip = Instance.soundList[(int)sound].Sound;
        if (clip != null)
        {
            Instance.audioSource.PlayOneShot(clip, volume);
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

