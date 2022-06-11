using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;
    public AudioMixerGroup channel;

    [Range(0.0001f, 1f)] public float volume;
    public bool loop;

    [System.NonSerialized]
    public AudioSource source;
}
