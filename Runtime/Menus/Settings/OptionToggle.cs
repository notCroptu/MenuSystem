using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class OptionToggle
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Toggle Toggle { get; private set; }

    // i think id only set this up through script, but Im leaving it as a serialized field for debugging.
    [field: SerializeField][HideInInspector]public UnityEvent<bool> OnValueChanged { get; private set; }

    public void Init()
    {
        if (Toggle != null)
            Toggle.onValueChanged.AddListener(OnValueChanged.Invoke);
    }
}
