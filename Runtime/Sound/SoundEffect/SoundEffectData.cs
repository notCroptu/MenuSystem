using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffectData", menuName = "Scriptable Objects/Sound Effect Data")]
public class SoundEffectData : ScriptableObject
{
    public string Name => this.name;

    [field: SerializeField] public AudioClip[] PossibleClips { get; private set; }
}