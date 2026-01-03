using System.Collections.Generic;
using MenuSystem.Settings;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager2D : Audio
{
    [SerializeField] private List<AudioSource> _audioSources;
    [SerializeField] private AudioMixerGroup _audioMixer;

    private void Start()
    {
        SetupSource(_audioSource);
    }

    private void SetupSource(AudioSource source)
    {
        ConnectMixer(Volume.SFX.ToName(), source);
        source.loop = false;
        source.spatialBlend = 0f;
    }

    private void Awake()
    {
        foreach (AudioSource source in _audioSources)
        {
            source.playOnAwake = false;
            source.outputAudioMixerGroup = _audioMixer;
        }
    }

    public AudioSource PlaySound(AudioClip clip)
{
    foreach (AudioSource source in _audioSources)
    {
        if (!source.isPlaying)
        {
            source.PlayOneShot(clip);
            return source;
        }
    }

    _audioSources.Add(gameObject.AddComponent<AudioSource>());
    AudioSource newSource = _audioSources[_audioSources.Count - 1];

    SetupSource(newSource);

    newSource.PlayOneShot(clip);
    return newSource;
    }
}
