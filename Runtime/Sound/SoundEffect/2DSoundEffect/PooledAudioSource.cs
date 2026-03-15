using UnityEngine;

public class PooledAudioSource
{
    public AudioSource Source;
    public SoundEffect2D Owner;
    public bool InUse => Source.isPlaying;
}