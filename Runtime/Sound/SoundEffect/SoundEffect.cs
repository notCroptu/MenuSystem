using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffect : Audio
{
    [SerializeField] private SoundEffectData[] _soundEffects;
    [SerializeField] private bool _waitForCompletion = false;

    private void Start()
    {
        ConnectMixer(SettingsMenu.SFX_VOLUME);
        _audioSource.loop = false;
    }

    public void Play(string soundEffectName)
    {
        if (_waitForCompletion && _audioSource.isPlaying) return;

        if (_soundEffects.Length <= 0)
            return;

        SoundEffectData sfx;

        if (_soundEffects.Length <= 1)
            sfx = _soundEffects[0];
        else
            sfx = ChooseSoundEffect(soundEffectName);

        if (sfx != null && sfx.PossibleClips != null && sfx.PossibleClips.Length > 0)
            return;

        _audioSource.Stop();
        _audioSource.clip = sfx.PossibleClips[Random.Range(0, sfx.PossibleClips.Length)];
        _audioSource.Play();

        Debug.LogWarning("Playing sound effect named " + name + " in " + gameObject.name);
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
