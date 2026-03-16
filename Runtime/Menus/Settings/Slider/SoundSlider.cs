using System;
using MenuSystem.Settings;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundSlider : SettingsSlider
{
    [HideInInspector] [SerializeField] private string Name;
    [SerializeField] private Volume _volumeType;

    private AudioMixer _masterMixer;
    private float _maxVolume;

    protected override string PrefKey => _volumeType.ToName();

    public void Init(AudioMixer masterMixer, float maxVolume)
    {
        _masterMixer = masterMixer;
        _maxVolume = maxVolume;

        base.Init();

        /* _masterMixer.GetFloat(_volumeType.ToName(), out float dB);
        _slider.value = PlayerPrefs.GetFloat(
            _volumeType.ToName(),
            SettingsMenu.DecibelToLinear(dB)); */
    }

    protected override float Apply(float value)
    {
        float final = _maxVolume *
                      (value - _slider.minValue) /
                      (_slider.maxValue - _slider.minValue);

        _masterMixer.SetFloat(
            _volumeType.ToName(),
            SettingsMenu.LinearToDecibel(final));

        return final;
    }

    public void Validate()
    {
        Name = _volumeType.ToName();
    }
}