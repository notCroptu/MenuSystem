using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : Audio
{

    private void Start()
    {
        ConnectMixer(SettingsMenu.MUSIC_VOLUME);
        _audioSource.loop = true;
        _audioSource.Play();
    }
}
