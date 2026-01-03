using System;
using MenuSystem.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[Serializable]
public class SoundSlider : ISetting
{
    [HideInInspector] [SerializeField] private string Name;
    [SerializeField] private Volume _volumeType;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _sliderText;

    private AudioMixer _masterMixer;
    private float _maxVolume;

    public void Init(AudioMixer masterMixer, float maxVolume)
    {
        _masterMixer = masterMixer;
        _maxVolume = maxVolume;

        if (_slider != null)
        {
            _slider.onValueChanged.AddListener(ChangeVolume);

            _masterMixer.GetFloat(_volumeType.ToName(), out float dB);
            _slider.value = PlayerPrefs.GetFloat(_volumeType.ToName(), SettingsMenu.DecibelToLinear(dB));
            LoadPref();
        }
        else
            Debug.LogWarning("Settings " + _volumeType.ToName() + " volume slider not assigned.");
    }

    public void End()
    {
        _slider?.onValueChanged.RemoveListener(ChangeVolume);
    }

    public void ChangeVolume(float value)
    {
        if (_slider == null) return;

        _masterMixer.SetFloat(
            _volumeType.ToName(),
            SettingsMenu.LinearToDecibel(
                GetVolume(value, _slider, _sliderText, _volumeType.ToName())));
    }

    private float GetVolume(float value, Slider slider, TMP_Text text, string prefsKey)
    {
        float final = _maxVolume * (value - slider.minValue)
            / (slider.maxValue - slider.minValue);

        if (text != null)
            text.text = SettingsMenu.FormatShort(final);

        PlayerPrefs.SetFloat(prefsKey, value);
        PlayerPrefs.Save();

        return final;
    }

    public void LoadPref()
    {
        if (_slider == null) return;
        _slider.value = PlayerPrefs.GetFloat(_volumeType.ToName(), 1f);
    }

    public void SavePref()
    {
        if (_slider == null) return;
        PlayerPrefs.SetFloat(_volumeType.ToName(), _slider.value);
        PlayerPrefs.Save();
    }

    public void Validate()
    {
        Name = _volumeType.ToName();
    }
}
