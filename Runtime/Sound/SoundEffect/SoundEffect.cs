using MenuSystem.Settings;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffect : Audio
{
    [SerializeField] private SoundEffectData[] _soundEffects;
    [SerializeField] private bool _waitForCompletion = false;
    [SerializeField] [ShowIf("_waitForCompletion")] private bool _toggleAudio = false;

    private void Start()
    {
        ConnectMixer(Volume.SFX.ToName());
        _audioSource.loop = false;
    }

    public void PlaySO(SoundEffectData sfx)
    {
        Debug.Log("TRY Playing SFX named " + sfx.name + " through GO " + gameObject.name);

        if (_waitForCompletion && _audioSource.isPlaying)
        {
            if (_toggleAudio && _audioSource.isPlaying)
                _audioSource.Stop();
            return;
        }

        if (sfx == null || sfx.PossibleClips == null || sfx.PossibleClips.Length <= 0)
            return;

        _audioSource.Stop();
        _audioSource.clip = sfx.PossibleClips[Random.Range(0, sfx.PossibleClips.Length)];
        _audioSource.Play();
        
        Debug.Log("Playing SFX named " + sfx.name + " through GO " + gameObject.name);
    }

    public void Play(string soundEffectName)
    {
        if (_soundEffects.Length <= 0)
            return;

        if (_waitForCompletion && _audioSource.isPlaying)
        {
            if (_toggleAudio && _audioSource.isPlaying)
                _audioSource.Stop();
            return;
        }

        SoundEffectData sfx;

        if (_soundEffects.Length <= 1)
            sfx = _soundEffects[0];
        else
            sfx = ChooseSoundEffect(soundEffectName);

        if (sfx == null || sfx.PossibleClips == null || sfx.PossibleClips.Length <= 0)
            return;

        _audioSource.Stop();
        _audioSource.clip = sfx.PossibleClips[Random.Range(0, sfx.PossibleClips.Length)];
        _audioSource.Play();
    }

    private SoundEffectData ChooseSoundEffect(string name)
    {
        foreach (SoundEffectData sfx in _soundEffects)
            if (sfx.Name == name)
                return sfx;

        Debug.LogWarning("Couldn't find sound effect named " + name + " in " + gameObject.name);

        return null;
    }

    public void Stop()
    {
        if (!_audioSource.isPlaying) return;

        _audioSource.Stop();
    }
}
