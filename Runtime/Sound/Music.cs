using MenuSystem.Settings;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : AudioBase
{

    private void Start()
    {
        ConnectMixer(Volume.MUSIC.ToName());
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
