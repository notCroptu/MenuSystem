using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Audio : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}