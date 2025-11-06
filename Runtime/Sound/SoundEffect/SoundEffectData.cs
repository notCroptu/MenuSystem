using System;
using UnityEngine;

[Serializable]
public class SoundEffectData
{
    [field:SerializeField] public string Name { get; private set;  }
    [field:SerializeField] public AudioClip[] PossibleClips  { get; private set;  }
}
