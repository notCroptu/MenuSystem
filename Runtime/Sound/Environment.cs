using MenuSystem.Settings;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Environment : AudioBase
{

    private void Start()
    {
        ConnectMixer(Volume.ENVIRONMENT.ToName());
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
