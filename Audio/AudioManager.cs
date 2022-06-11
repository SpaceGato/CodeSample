using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioMixerGroup musicGroup, soundGroup;
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.channel;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
        }
    }
    private void Start()
    {
        InitializeMixerVolume();        
    }
    private void InitializeMixerVolume()
    {
        float mVol = PlayerPrefs.GetFloat("musicVol", 0.5f);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(mVol) * 20);

        float sVol = PlayerPrefs.GetFloat("soundVol", 0.6f);
        audioMixer.SetFloat("soundVolume", Mathf.Log10(sVol) * 20);
    }

    public AudioSource GetAudioSource(string name)
    {
        Sound track = Array.Find(sounds, sound => sound.name == name);
        if (track == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
        else
            return track.source;
    }

    public void Play(string name)
    {
        Sound track = Array.Find(sounds, sound => sound.name == name);
        if (track == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        track.source.Play();
    }
    public void ChangeTrack(string newTrack)
    {
        ReplaceMusic(newTrack);
    }  
    
    public void FadeLevelMusic(float time)
    {
        AudioSource level = GetAudioSource("LevelTrack");
        AudioSource boss = GetAudioSource("BossTrack");
        if (level.isPlaying)
            StartCoroutine(FadeOut(level, time));
        if (boss.isPlaying)
            StartCoroutine(FadeOut(boss, time));
    }
    public void FadeAll(float time, bool music)
    {
        AudioMixerGroup group;
        if (music)
            group = musicGroup;
        else
            group = soundGroup;

        foreach (Sound track in sounds)
        {
            if (track.source.isPlaying && track.channel == group)
                    StartCoroutine(FadeOut(track.source, time));
        }
    }

    private void ReplaceMusic(string newTrack)
    {
        FadeAll(0.5f, true);
        Play(newTrack);
    }

    private IEnumerator FadeOut(AudioSource source, float fadeTime)
    {
        float startVolume = source.volume;
        while (source.volume > 0.0001f)
        {
            source.volume -= startVolume * Time.unscaledDeltaTime / fadeTime;
            yield return null;
        }
        source.volume = 0.0001f;
        source.Stop();
        source.volume = 1;
    }
}