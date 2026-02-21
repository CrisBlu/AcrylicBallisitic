using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioAsset
{
    public string name;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "NewAudioAsset", menuName = "Audio/AudioAsset")]
public class AudioAssetSO : ScriptableObject
{
    public List<AudioAsset> audioAssets;
}