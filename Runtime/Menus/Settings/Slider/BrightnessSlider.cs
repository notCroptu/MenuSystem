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

    private LiftGammaGain _gamma;

    protected override string PrefKey => "Brightness";

    public override void Init()
    {
        base.Init();

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

        float normalized = Mathf.InverseLerp(
            _minMaxBrightness.x,
            _minMaxBrightness.y,
            _gamma.gamma.value.w);

        _slider.value = Mathf.Lerp(_slider.minValue, _slider.maxValue, normalized);
    }

    protected override float Apply(float value)
    {
        if (_gamma == null) return value;

        float normalized = Mathf.InverseLerp(_slider.minValue, _slider.maxValue, value);
        float final = Mathf.Lerp(_minMaxBrightness.x, _minMaxBrightness.y, normalized);

        Vector4 gamma = _gamma.gamma.value;
        gamma.w = final;
        _gamma.gamma.Override(gamma);

        return final;
    }
}