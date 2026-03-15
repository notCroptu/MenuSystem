using NaughtyAttributes;
using UnityEngine;

public class SoundEffect2D : MonoBehaviour
{
    [SerializeField] private SoundEffectData[] _soundEffects;
    [SerializeField] private bool _waitForCompletion = false;
    [SerializeField] private bool _StopWhenPaused = true;
    [SerializeField] [ShowIf("_waitForCompletion")] private bool _toggleAudio = false;
    
    private SoundManager2D _soundManager;
    private AudioSource _audioSource;

    public void Play(string soundEffectName)
    {
        if (_soundEffects.Length <= 0)
            return;

        if (_soundManager == null)
            _soundManager = FindFirstObjectByType<SoundManager2D>();

        if (_soundManager == null)
        {
            Debug.Log("Couldn't find sound manager. ");
            return;
        }

        if (_audioSource != null && _waitForCompletion && _audioSource.isPlaying)
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
            
        AudioClip clip = sfx.PossibleClips[Random.Range(0, sfx.PossibleClips.Length)];

        if (_StopWhenPaused)
            _audioSource = _soundManager.PlayGameplaySound(clip, this);
        else
            _audioSource = _soundManager.PlayUISound(clip, this);
    }

    public void DisownSource()
    {
        _audioSource = null;
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
