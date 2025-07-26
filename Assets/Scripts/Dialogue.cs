using System;
using UnityEngine;

[Serializable]
public class Dialogue // of civilizations 
{
    [TextArea(3, 5)]
    [SerializeField] public string text;
    [SerializeField] public Sprite sprite;
    [SerializeField] public AudioClip audio;
}
