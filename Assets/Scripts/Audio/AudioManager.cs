using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;

    public Sound[] music;
    public Sound[] soundEffects;
    public Sound bgm;

    private void Awake()
    {
        bgm.source = gameObject.AddComponent<AudioSource>();
        bgm.source.outputAudioMixerGroup = musicMixer;

        foreach (Sound s in soundEffects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.outputAudioMixerGroup = sfxMixer;


            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }

    private void Start()
    {
        GetVolumeSettings();
    }

    private void GetVolumeSettings()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0);
        audioMixer.SetFloat("musicVolume", musicVolume);

        float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0);
        audioMixer.SetFloat("sfxVolume", sfxVolume);
    }

    public void ChangeBGM(string name)
    {
        StopBGM();

        Sound m = Array.Find(music, sound => sound.name == name);

        if (m == null)
        {
            Debug.LogWarning("Track: " + name + " not found!");
            return;
        }

        bgm.source.clip = m.clip;
        bgm.source.volume = m.volume;
        bgm.source.pitch = m.pitch;
        bgm.source.loop = m.loop;

        bgm.source.Play();
    }

    public void StopBGM ()
    {
        if (!bgm.source.isPlaying)
        {
            Debug.Log("No BGM to stop.");
            return;
        }

        bgm.source.Stop();
    }

    public void Play (string name)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}
