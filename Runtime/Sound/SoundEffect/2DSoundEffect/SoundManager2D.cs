using System.Collections.Generic;
using MenuSystem.Settings;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundManager2D : AudioBase
{
    [SerializeField] private AudioMixerGroup _audioMixer;
    private List<PooledAudioSource> _gameplaySources;
    private List<PooledAudioSource> _uiSources;

    private void Awake()
    {
        _gameplaySources = new List<PooledAudioSource>();
        _uiSources = new List<PooledAudioSource>();
    }

    private void ConfigureSource(AudioSource source, bool timeScaled = true)
    {
        ConnectMixer(Volume.SFX.ToName(), source);
        source.loop = false;
        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = _audioMixer;
        source.pitch = timeScaled ? Mathf.Clamp(Time.timeScale, 0f, 1f) : 1f;
    }

    private void Update()
    {
        float scaledPitch = Mathf.Clamp(Time.timeScale, 0f, 1f);

        if (_gameplaySources != null)
        {
            foreach (PooledAudioSource source in _gameplaySources)
            {
                if (source != null)
                    source.Source.pitch = scaledPitch;
            }
        }
    }


    public AudioSource PlayGameplaySound(AudioClip clip, SoundEffect2D sfx2D) => PlaySound(_gameplaySources, clip, sfx2D);
    public AudioSource PlayUISound(AudioClip clip, SoundEffect2D sfx2D) => PlaySound(_uiSources, clip, sfx2D);
    private AudioSource PlaySound(List<PooledAudioSource> sources, AudioClip clip, SoundEffect2D sfx2D)
    {
        PooledAudioSource source = null;

        foreach (PooledAudioSource s in sources)
        {
            if (!s.InUse)
            {
                source = s;
                if (s.Owner != null)
                    s.Owner.DisownSource();
                break;
            }
        }

        if (source == null)
        {
            source = new PooledAudioSource { Source = gameObject.AddComponent<AudioSource>() };
            bool isTimeScaled = sources == _gameplaySources;
            ConfigureSource(source.Source, isTimeScaled);
            sources.Add(source);
        }

        source.Owner = sfx2D;
        source.Source.clip = clip;
        source.Source.pitch = sources == _gameplaySources ? Mathf.Clamp(Time.timeScale, 0f, 1f) : 1f;
        source.Source.Play();
        
        return source.Source;
    }
}
