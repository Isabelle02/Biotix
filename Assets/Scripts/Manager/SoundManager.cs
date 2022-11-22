using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private const string SoundKey = "Sound";
    
    private static SoundManager _instance;

    public static bool IsOn
    {
        get => PlayerPrefs.GetInt(SoundKey, 0) == 1;
        set
        {
            PlayerPrefs.SetInt(SoundKey, value ? 1 : 0);
            _instance._audioSource.volume = value ? 1 : 0;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);

        _audioSource.volume = IsOn ? 1 : 0;
    }

    public static float GetClipLength(Sound sound)
    {
        return SoundClipConfig.GetClip(sound).length;
    }

    public static void Play(Sound sound)
    {
        _instance._audioSource.clip = SoundClipConfig.GetClip(sound);
        _instance._audioSource.Play();
    }

    public static void PlayOneShot(Sound sound)
    {
        _instance._audioSource.PlayOneShot(SoundClipConfig.GetClip(sound));
    }
}