using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "SoundClipConfig", menuName = "Configs/SoundClipConfig")]
public class SoundClipConfig : ScriptableObject
{
    [SerializeField] private SerializedDictionary<Sound, AudioClip> _sounds;

    private static SoundClipConfig _instance;

    private static SoundClipConfig Instance
    {
        get
        {
            if (_instance == null) 
                _instance = Resources.Load<SoundClipConfig>("Configs/SoundClipConfig");

            return _instance;
        }
    }
    
    public static AudioClip GetClip(Sound sound)
    {
        return Instance._sounds[sound];
    }
}

public enum Sound
{
    Space,
    Toggle
}