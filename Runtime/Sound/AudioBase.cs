using MenuSystem.Settings;
using UnityEngine;
using UnityEngine.Audio;

public abstract class AudioBase : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;

    private void Awake()
    {
        if (_audioSource == null)
            _audioSource = GetComponent<AudioSource>();
    }

    protected void ConnectMixer(string mixerGroupName, AudioSource audioSource = null)
    {
        if (audioSource != null)
            _audioSource = audioSource;
        
        if (_audioSource.outputAudioMixerGroup == null)
        {
            AudioMixer mixer = Resources.Load<AudioMixer>(Volume.MASTER.ToName());
            if (mixer != null)
            {
                AudioMixerGroup[] groups = mixer.FindMatchingGroups(mixerGroupName);
                if (groups.Length > 0)
                    _audioSource.outputAudioMixerGroup = groups[0];
                else
                    Debug.LogWarning("Could not find " + mixerGroupName + " mixer group in " + Volume.MASTER.ToName() + ". ");
            }
            else
            {
                Debug.LogWarning("Could not find " + Volume.MASTER.ToName() + " in Resources. ");
            }
        }
    }
}