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
        return SoundClip.GetClip(sound).length;
    }

    public static void Play(Sound sound)
    {
        _instance._audioSource.clip = SoundClip.GetClip(sound);
        _instance._audioSource.Play();
    }

    public static void PlayOneShot(Sound sound)
    {
        _instance._audioSource.PlayOneShot(SoundClip.GetClip(sound));
    }
}

public enum Sound
{
    Space,
    Toggle
}

[Serializable]
[CreateAssetMenu(fileName = "SoundClipConfig", menuName = "Configs/SoundClipConfig")]
public class SoundClip : ScriptableObject
{
    [SerializeField] private SerializedDictionary<Sound, AudioClip> _sounds;

    private static SoundClip _instance;

    private static SoundClip Instance
    {
        get
        {
            if (_instance == null) 
                _instance = Resources.Load<SoundClip>("Configs/SoundClipConfig");

            return _instance;
        }
    }
    
    public static AudioClip GetClip(Sound sound)
    {
        return Instance._sounds[sound];
    }
}