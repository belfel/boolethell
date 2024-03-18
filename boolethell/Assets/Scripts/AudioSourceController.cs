using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool playsMusic;

    [ConditionalField(nameof(playsMusic))] 
    public FloatVariable bgmVolume;

    [ConditionalField(nameof(playsMusic), inverse:true)] 
    public FloatVariable sfxVolume;

    public void UpdateVolume()
    {
        if (playsMusic)
            audioSource.volume = AudioListener.volume * bgmVolume.Value;
        else audioSource.volume = AudioListener.volume * sfxVolume.Value;
    }
}
