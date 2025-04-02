using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SoundSlot
{
    public AudioClip sound;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3f, 3f)] public float pitch = 1f;
}
