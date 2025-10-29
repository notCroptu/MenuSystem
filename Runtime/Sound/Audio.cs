using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public abstract class Audio : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    protected void ConnectMixer(string mixerGroupName)
    {
        if (_audioSource.outputAudioMixerGroup == null)
        {
            AudioMixer mixer = Resources.Load<AudioMixer>("MasterMixer");
            if (mixer != null)
            {
                AudioMixerGroup[] groups = mixer.FindMatchingGroups(mixerGroupName);
                if (groups.Length > 0)
                    _audioSource.outputAudioMixerGroup = groups[0];
                else
                    Debug.LogWarning("Could not find " + mixerGroupName + " mixer group in MasterMixer. ");
            }
            else
            {
                Debug.LogWarning("Could not find MasterMixer in Resources. ");
            }
        }
    }
}