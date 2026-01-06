using MenuSystem.Settings;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : Menu
{
    [InfoBox("The settings game object must not be the same as the setting script's. \n Settings menu needs to be contained in a DDOL object (don't destroy on load (new scene) ).")]
    [Header("Settings")]

    [SerializeField] private BrightnessSlider _brightness;

    [Header("General Volume")]
    [SerializeField] private float _maxVolume = 1f;

    [SerializeField] private AudioMixer _masterMixer;

    [SerializeField] private SoundSlider[] _soundSliders;

    [Header("Option Toggles")]
    [SerializeField] private OptionToggle[] _optionToggles;

    // I think i can change music and sound effect change the volumes of a music and a sfx volume thingie on the volume mixer and therefore evading statics and findfirstof

    // TODO: mouse sensitivity

    private void OnEnable()
    {
        _brightness.Init();

        foreach (SoundSlider sound in _soundSliders)
            sound.Init(_masterMixer, _maxVolume);

        foreach (OptionToggle option in _optionToggles)
            option.Init();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("New Scene Loaded: " + scene.name);
        Start();
    }
    private void Start()
    {
        Continue();
    }

    public void TurnOnSettings()
    {
        if (_menuCanvas != null)
            _menuCanvas.gameObject.SetActive(true);
        else
            Debug.LogWarning(name + " _settingsCanvas not assigned.");
    }

    public override void Continue()
    {
        if (_menuCanvas != null)
            _menuCanvas.gameObject.SetActive(false);
    }

    private float GetVolume(float value, Slider slider, TMP_Text text, string prefsKey)
    {
        float final = _maxVolume * (value - slider.minValue)
            / (slider.maxValue - slider.minValue);

        if (text != null)
            text.text = FormatShort(final);

        PlayerPrefs.SetFloat(prefsKey, value);
        PlayerPrefs.Save();

        return final;
    }

    public static string FormatShort(float value)
    {
        if (value >= 10f)
            return Mathf.RoundToInt(value).ToString();
        else
            return value.ToString("0.0");
    }

    // audio mixers expect decibels
    public static float DecibelToLinear(float dB) => Mathf.Pow(10f, dB / 20f);
    public static float LinearToDecibel(float linear) => Mathf.Log10(Mathf.Clamp(linear, 0.001f, 1f)) * 20f;

    public void OnDestroy()
    {
        _brightness.End();

        foreach (SoundSlider sound in _soundSliders)
            sound.End();

        foreach (OptionToggle option in _optionToggles)
            option.End();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (SoundSlider sound in _soundSliders)
            sound.Validate();

        foreach (OptionToggle option in _optionToggles)
            option.Validate();
    }
#endif
}