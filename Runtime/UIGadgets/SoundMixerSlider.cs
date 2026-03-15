using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;
    [SerializeField] private string prefsKey = "MasterVolume";

    private void Awake()
    {
        float savedValue = PlayerPrefs.GetFloat(prefsKey, 1f);
        SetVolume(savedValue);

        slider.minValue = 0f;
        slider.maxValue = 1f;

        slider.value = savedValue;
        slider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float value)
    {
        float dB = Mathf.Log10(value) * 20f;
        mixer.SetFloat("Volume", dB);

        PlayerPrefs.SetFloat(prefsKey, value);
        PlayerPrefs.Save();
    }
}
