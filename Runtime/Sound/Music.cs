using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : Audio
{
    private float _volume;
    
    public float Volume
    {
        get { return _volume; }
        set
        {
            Debug.Log("Set music volume at: " + value);
            _audioSource.volume = value;
            _volume = value;
        }
    }

    private void Start()
    {
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
