using MenuSystem.Settings;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomEnvironment : AudioBase
{
    [SerializeField] private AudioClip[] _possibleClips;
    [SerializeField][Min(0f)] private float _waitTime = 30f;
    [SerializeField][Range(0f, 1f)] private float _playingProbability;
    [SerializeField] private bool _waitForCompletion = false;

    private float _time = 0f;

    private void Start()
    {
        ConnectMixer(Volume.ENVIRONMENT.ToName());
        _audioSource.loop = false;
    }

    private void Update()
    {
        if (_time > _waitTime)
        {
            if (Random.Range(0f, 1f) <= _playingProbability)
            {
                if (_possibleClips.Length <= 0)
                    return;
                
                if (_waitForCompletion && _audioSource.isPlaying)
                    _audioSource.Stop();

                AudioClip sfx = _possibleClips[Random.Range(0, _possibleClips.Length)];

                if (sfx == null)
                    return;

                _audioSource.Stop();
                _audioSource.clip = sfx;
                _audioSource.Play();
            }
            
            _time = 0f;
        }

        _time += Time.deltaTime;
    }
}
