using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[Serializable]
public class BrightnessSlider : ISetting
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Volume _postProcessWithGamma;
    [SerializeField][MinMaxSlider(-1f, 1f)] private Vector2 _minMaxBrightness = new(-0.5f, 0.5f);
    private LiftGammaGain _gamma;

    [BoxGroup("Slider Text")]
    [SerializeField] private TMP_Text _sliderText;
    [BoxGroup("Slider Text")]
    [SerializeField] private bool _percentText = false;

    public void Init()
    {

        if (_slider != null)
        {
            if (_postProcessWithGamma != null)
            {
                if (_postProcessWithGamma.profile.TryGet(out _gamma))
                {
                    _slider.onValueChanged.AddListener(ChangeBrightness);

                    float normalized = Mathf.InverseLerp(_minMaxBrightness.x, _minMaxBrightness.y, _gamma.gamma.value.w);
                    _slider.value = Mathf.Lerp(_slider.minValue, _slider.maxValue, normalized);
                }
                else
                    Debug.LogWarning("Settings gamma reference missing in post process, brightness adjustments disabled.");
            }
            else
                Debug.LogWarning("Settings post process reference missing, brightness adjustments disabled.");
        }
        else
            Debug.LogWarning("Settings brightness slider not assigned.");
    }

    public void End()
    {
        _slider?.onValueChanged.RemoveListener(ChangeBrightness);
    }

    public void ChangeBrightness(float value)
    {
        Debug.Log("Attempting brightness change. ");
        if (_gamma == null || _slider == null) return;

        float normalized = Mathf.InverseLerp(_slider.minValue, _slider.maxValue, value);
        float final = Mathf.Lerp(_minMaxBrightness.x, _minMaxBrightness.y, normalized);  // clamp between volume gamma's best values

        // gamma from post processing actually maps only the value.w for brightness, between the values of -1 and 1

        Vector4 newGamma = _gamma.gamma.value;
        newGamma.w = final;
        _gamma.gamma.Override(newGamma);

        if (_sliderText != null)
        {
            if (_percentText)
            {
                float percent = Mathf.InverseLerp(_slider.minValue, _slider.maxValue, value) * 100f;
                _sliderText.text = Mathf.RoundToInt(percent).ToString();
            }
            else
            {
                _sliderText.text = SettingsMenu.FormatShort(final);
            }
        }

        Debug.Log("Changing brightness from value " + value + " to: " + _gamma.gamma.value + ". ");
    }
    
    public void LoadPref()
    {
        if (_slider == null) return;
        _slider.value = PlayerPrefs.GetFloat("Brightness", 1f);
    }

    public void SavePref()
    {
        if (_slider == null) return;
        PlayerPrefs.SetFloat("Brightness", _slider.value);
        PlayerPrefs.Save();
    }
}
