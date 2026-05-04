using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public class BrightnessSlider : SettingsSlider
{
    [SerializeField] private Volume _postProcessWithGamma;
    [SerializeField][MinMaxSlider(-2f, 2f)] private Vector2 _minMaxBrightness = new(-0.5f, 0.5f);
    [SerializeField] private VolumeProfile _defaultProcessWithGamma;

    private LiftGammaGain _defaultGamma;
    private LiftGammaGain _gamma;

    protected override string PrefKey => "Brightness";

    public override void Init()
    {
        if (_postProcessWithGamma == null)
        {
            Debug.LogWarning("Settings post process reference missing.");
            return;
        }

        if (!_postProcessWithGamma.profile.TryGet(out _gamma))
        {
            Debug.LogWarning("Settings gamma reference missing.");
            return;
        }

        if (!_defaultProcessWithGamma.TryGet(out _defaultGamma))
        {
            Debug.LogWarning("Couldn't get default Gamma values.");

            _gamma.lift.Override(_defaultGamma.lift.value);
            _gamma.gamma.Override(_defaultGamma.gamma.value);
            _gamma.gain.Override(_defaultGamma.gain.value);
        }

        base.Init();

        /*float normalized = Mathf.InverseLerp(
            _minMaxBrightness.x,
            _minMaxBrightness.y,
            _gamma.gamma.value.w);

        _slider.value = Mathf.Lerp(_slider.minValue, _slider.maxValue, normalized);*/
    }

    protected override float Apply(float value)
    {
        if (_gamma == null) return value;

        float normalized = Mathf.InverseLerp(_slider.minValue, _slider.maxValue, value);
        float final;

        if (_defaultGamma != null)
        {
            float defaultGamma = _defaultGamma.gamma.value.w;
            final = Mathf.Lerp(defaultGamma + _minMaxBrightness.x, defaultGamma + _minMaxBrightness.y, normalized);
        }
        else
            final = Mathf.Lerp(_minMaxBrightness.x, _minMaxBrightness.y, normalized);

        Vector4 gamma = _gamma.gamma.value;
        gamma.w = final;
        _gamma.gamma.Override(gamma);

        return final;
    }
}