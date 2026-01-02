using MenuSystem.Settings;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Environment : Audio
{

    private void Start()
    {
        ConnectMixer(Volume.ENVIRONMENT.ToName());
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
