using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : Menu
{
    [InfoBox("The settings game object must not be the same as the setting script's. \n Settings menu needs to be contained in a DDOL object (don't destroy on load (new scene) ).")]
    [Header("Settings")]

    [Header("Brightness")]
    [SerializeField] private Slider _brightness;
    [SerializeField] private TMP_Text _brightnessText;
    [SerializeField] private Volume volume;
    [SerializeField] private float _clampBrightness = 2.5f;
    private ColorAdjustments _postExposure;

    [Header("General Volume")]
    [SerializeField] private Slider _volume;
    [SerializeField] private TMP_Text _volumeText;
    [SerializeField] private float _maxVolume = 3f;
    
    private AudioMixer _masterMixer;

    [Header("Music Volume")]
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private TMP_Text _musicVolumeText;
    public const string MUSIC_VOLUME = "MusicVolume";

    [Header("SFX Volume")]
    [SerializeField] private Slider _sfxVolume;
    [SerializeField] private TMP_Text _sfxVolumeText;
    public const string SFX_VOLUME = "SFXVolume";


    // TODO: sound effect volume
    // I think i can change music and sound effect change the volumes of a music and a sfx volume thingie on the volume mixer and therefore evading statics and findfirstof

    // TODO: mouse sensitivity

    private void Awake()
    {
        _masterMixer = Resources.Load<AudioMixer>("MasterMixer");
        if (_masterMixer == null)
            Debug.LogWarning("Could not find MasterMixer in Resources. ");

        if (volume == null)
            Debug.LogWarning(name + " volume reference missing, brightness adjustments disabled.");
        else
            volume.profile?.TryGet(out _postExposure);

        if (_brightness == null)
            Debug.LogWarning(name + " brightness slider not assigned.");
        else
            _brightness.onValueChanged.AddListener(ChangeBrightness);

        if (_volume == null)
            Debug.LogWarning(name + " general volume slider not assigned.");
        else
            _volume.onValueChanged.AddListener(ChangeVolume);

        if (_musicVolume == null)
            Debug.LogWarning(name + " music volume slider not assigned.");
        else
            _musicVolume.onValueChanged.AddListener(ChangeMusicVolume);

        if (_sfxVolume == null)
            Debug.LogWarning(name + " sfx volume slider not assigned.");
        else
            _sfxVolume.onValueChanged.AddListener(ChangeSFXVolume);
    }

    private void OnEnable()
    {
        if (_volume != null) _volume.value = 1f;
        if (_musicVolume != null) _musicVolume.value = 1f;
        if (_brightness != null) _brightness.value = 0.5f;

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
        if (_brightness != null) ChangeBrightness(_brightness.value);
        if (_volume != null) ChangeVolume(_volume.value);
        if (_musicVolume != null) ChangeMusicVolume(_musicVolume.value);
        if (_sfxVolume != null) ChangeSFXVolume(_sfxVolume.value);

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
        if ( _menuCanvas != null )
            _menuCanvas.gameObject.SetActive(false);
    }

    public void ChangeBrightness(float value)
    {
        if (_postExposure == null || _brightness == null) return;
        
        float final =  _clampBrightness * (value - _brightness.minValue)
            / (_brightness.maxValue - _brightness.minValue);

        _postExposure.postExposure.value =  final;
        
        if (_brightnessText != null)
            _brightnessText.text = FormatShort(final);
    }

    public void ChangeVolume(float value)
    {
        if (_volume == null) return;

        float final = _maxVolume * (value - _volume.minValue)
            / (_volume.maxValue - _volume.minValue);

        AudioListener.volume = final;

        if (_volumeText != null)
            _volumeText.text = FormatShort(final);
    }
    public void ChangeMusicVolume(float value)
    {
        if (_musicVolume == null) return;

        float final = _maxVolume * (value - _musicVolume.minValue)
            / (_musicVolume.maxValue - _musicVolume.minValue);

        _masterMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(Mathf.Clamp(final, 0.001f, 1f)) * 20f);
        
        _musicVolumeText?.SetText(FormatShort(final));
    }

    public void ChangeSFXVolume(float value)
    {
        if (_sfxVolume == null) return;

        float final = _maxVolume * (value - _sfxVolume.minValue)
            / (_sfxVolume.maxValue - _sfxVolume.minValue);

        _masterMixer.SetFloat(SFX_VOLUME, Mathf.Log10(Mathf.Clamp(final, 0.001f, 1f)) * 20f);
        
        _sfxVolumeText?.SetText(FormatShort(final));
    }

    private string FormatShort(float value)
    {
        if (value >= 10f)
            return Mathf.RoundToInt(value).ToString();
        else
            return value.ToString("0.0");
    }

    public void OnDestroy()
    {
        _brightness?.onValueChanged.RemoveListener(ChangeBrightness);
        _volume?.onValueChanged.RemoveListener(ChangeVolume);
        _musicVolume?.onValueChanged.RemoveListener(ChangeMusicVolume);
        _sfxVolume?.onValueChanged.RemoveListener(ChangeSFXVolume);
    }
}