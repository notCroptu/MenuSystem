using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SettingsSlider : ISetting
{
    [SerializeField] protected Slider _slider;

    [Header("Slider Text")]
    [SerializeField] protected TMP_Text _sliderText;

    [SerializeField] protected bool _showCustomRange = false;

    [AllowNesting]
    [ShowIf(nameof(_showCustomRange))]
    [BoxGroup("Custom Range")]
    [SerializeField] protected Vector2 _displayRange = new(0, 100);

    [AllowNesting]
    [ShowIf(nameof(_showCustomRange))]
    [BoxGroup("Custom Range")]
    [SerializeField] protected bool _showDecimal = false;
    
    protected virtual string PrefKey => GetType().Name;

    public virtual void Init()
    {
        if (_slider == null)
        {
            Debug.LogWarning($"{GetType().Name} slider not assigned.");
            return;
        }

        _slider.onValueChanged.AddListener(OnSliderChanged);
        LoadPref();

        float final = Apply(_slider.value);
        UpdateText(_slider.value, final);
    }

    public virtual void End()
    {
        _slider?.onValueChanged.RemoveListener(OnSliderChanged);
    }

    protected void UpdateText(float value, float finalValue)
    {
        if (_sliderText == null) return;

        if (_showCustomRange)
        {
            float normalized = Mathf.InverseLerp(_slider.minValue, _slider.maxValue, value);
            finalValue = Mathf.Lerp(_displayRange.x, _displayRange.y, normalized);

            if (!_showDecimal)
                finalValue = Mathf.Round(finalValue);

            _sliderText.text = _showDecimal
                ? finalValue.ToString("0.##")
                : ((int)finalValue).ToString();
        }
        else
            _sliderText.text = SettingsMenu.FormatShort(finalValue);
    }

    protected virtual void OnSliderChanged(float value)
    {
        float final = Apply(value);
        UpdateText(value, final);
        SavePref();
    }

    protected abstract float Apply(float value);

    public virtual void LoadPref()
    {
        if (_slider == null) return;
        _slider.value = PlayerPrefs.GetFloat(PrefKey, _slider.value);
        Debug.Log("Load pref: " + PrefKey + " with value: " + _slider.value);
    }

    public virtual void SavePref()
    {
        if (_slider == null) return;
        Debug.Log("Save pref: " + PrefKey + " with value: " + _slider.value);
        PlayerPrefs.SetFloat(PrefKey, _slider.value);
        PlayerPrefs.Save();
    }
}