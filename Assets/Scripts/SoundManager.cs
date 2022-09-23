using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _spaceClip;
    [SerializeField] private AudioClip _toggleClip;

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
        Play(Sound.Space);
    }

    public static float GetClipLength(Sound sound)
    {
        return sound switch
        {
            Sound.Space => _instance._spaceClip.length,
            Sound.Toggle => _instance._toggleClip.length,
            _ => 0f
        };
    }

    public static void Play(Sound sound)
    {
        switch (sound)
        {
            case Sound.Space:
                _instance._audioSource.clip = _instance._spaceClip;
                _instance._audioSource.Play();
                break;
            case Sound.Toggle:
                _instance._audioSource.PlayOneShot(_instance._toggleClip);
                break;
        }
    }
}

public enum Sound
{
    Space,
    Toggle
}