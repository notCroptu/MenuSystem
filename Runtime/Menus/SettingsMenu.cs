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
    [SerializeField] private Volume _postProcessWithGamma;
    [SerializeField][MinMaxSlider(-1f, 1f)] private Vector2 _minMaxBrightness = new(-0.5f, 0.5f);
    private LiftGammaGain _gamma;

    [Header("General Volume")]
    [SerializeField] private Slider _volume;
    [SerializeField] private TMP_Text _volumeText;
    [SerializeField] private float _maxVolume = 2f;
    
    private AudioMixer _masterMixer;
    public const string MASTER_MIXER = "MasterMixer";

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
        _masterMixer = Resources.Load<AudioMixer>(MASTER_MIXER);
        if (_masterMixer == null)
            Debug.LogWarning("Could not find " + MASTER_MIXER + " in Resources. ");
    }

    private void OnEnable()
    {
        if (_volume != null)
        {
            _volume.onValueChanged.AddListener(ChangeVolume);

            _volume.value = _volume.maxValue * AudioListener.volume / _maxVolume;
        }
        else
            Debug.LogWarning(name + " post process slider not assigned.");
        
        if (_musicVolume != null)
        {
            _musicVolume.onValueChanged.AddListener(ChangeMusicVolume);

            _masterMixer.GetFloat(MUSIC_VOLUME, out float dB);
            _musicVolume.value = Mathf.Pow(10f, dB / 20f);
        }
        else
            Debug.LogWarning(name + " music volume slider not assigned.");

        if (_sfxVolume != null)
        {
            _sfxVolume.onValueChanged.AddListener(ChangeSFXVolume);

            _masterMixer.GetFloat(SFX_VOLUME, out float dB);
            _sfxVolume.value = Mathf.Pow(10f, dB / 20f);
        }
        else
            Debug.LogWarning(name + " sfx volume slider not assigned.");

        if (_brightness != null)
        {
            if (_postProcessWithGamma != null)
            {
                if (_postProcessWithGamma.profile.TryGet(out _gamma))
                {
                    _brightness.onValueChanged.AddListener(ChangeBrightness);

                    float normalized = Mathf.InverseLerp(_minMaxBrightness.x, _minMaxBrightness.y, _gamma.gamma.value.w);
                    _brightness.value = Mathf.Lerp(_brightness.minValue, _brightness.maxValue, normalized);
                }
                else
                    Debug.LogWarning(name + " gamma reference missing in post process, brightness adjustments disabled.");                
            }
            else
                Debug.LogWarning(name + " post process reference missing, brightness adjustments disabled.");
        }
        else
            Debug.LogWarning(name + " brightness slider not assigned.");

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
        if ( _menuCanvas != null )
            _menuCanvas.gameObject.SetActive(false);
    }

    public void ChangeBrightness(float value)
    {
        Debug.Log("Attempting brightness change. ");
        if (_gamma == null || _brightness == null) return;

        float normalized = Mathf.InverseLerp(_brightness.minValue, _brightness.maxValue, value);
        float final = Mathf.Lerp(_minMaxBrightness.x, _minMaxBrightness.y, normalized);  // clamp between volume gamma's best values

        // gamma from post processing actually maps only the value.w for brightness, between the values of -1 and 1

        Vector4 newGamma = _gamma.gamma.value;
        newGamma.w = final;
        _gamma.gamma.Override(newGamma);

        if (_brightnessText != null)
            _brightnessText.text = FormatShort(final);
        
        Debug.Log("Changing brightness from value " + value + " to: " + _gamma.gamma.value + ". ");
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

        _masterMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(Mathf.Clamp(final, 0.001f, 1f)) * 20f); // audio mixers expect decibels
        
        _musicVolumeText?.SetText(FormatShort(final));
    }

    public void ChangeSFXVolume(float value)
    {
        if (_sfxVolume == null) return;

        float final = _maxVolume * (value - _sfxVolume.minValue)
            / (_sfxVolume.maxValue - _sfxVolume.minValue);

        _masterMixer.SetFloat(SFX_VOLUME, Mathf.Log10(Mathf.Clamp(final, 0.001f, 1f)) * 20f); // audio mixers expect decibels
        
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