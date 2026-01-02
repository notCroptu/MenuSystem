using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewOptionSettings", menuName = "Settings/Option Settings")]
public class OptionSettings : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    private bool _value;

    // i think id only set this up through script, but Im leaving it as a serialized field for debugging.
    public UnityEvent<bool> OnValueChanged;


    public void Toggle()
    {
        _value = !_value;
        OnValueChanged?.Invoke(_value);
    }

    public void SetToggle(bool value)
    {
        _value = value;
        OnValueChanged?.Invoke(_value);
    }

    public bool CheckToggle()
    {
        return _value;
    }
}
