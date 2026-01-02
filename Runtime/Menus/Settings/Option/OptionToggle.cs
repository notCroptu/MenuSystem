using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class OptionToggle : ISetting
{
    [field: SerializeField] public OptionSettings Settings { get; private set; }
    [field: SerializeField] public Toggle Toggle { get; private set; }

    // i think id only set this up through script, but Im leaving it as a serialized field for debugging.
    [field: SerializeField][HideInInspector] public UnityEvent<bool> OnValueChanged { get; private set; }

    public void Init()
    {
        if (Settings == null)
        {
            Debug.LogWarning("An option toggle in settings menu has no settings object reference. ");
            return;
        }

        LoadPref();
        Settings.SetToggle(Toggle.isOn);
        if (Toggle != null)
            Toggle.onValueChanged.AddListener(ToggleSettings);
    }

    public void ToggleSettings(bool value)
    {
        if (Settings == null)
        {
            Debug.LogWarning("An option toggle in settings menu has no settings object reference. ");
            return;
        }

        Settings.Toggle();
        SavePref();
    }
    
    public void LoadPref()
    {
        Toggle.isOn = PlayerPrefs.GetInt(Settings.Name, 0) == 1;
    }

    public void SavePref()
    {
        PlayerPrefs.SetInt(Settings.Name, Toggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
