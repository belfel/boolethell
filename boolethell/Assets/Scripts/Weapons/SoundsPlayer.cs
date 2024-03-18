using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClipOnce()
    {
        if (!audioSource)
            return;

        int r = Random.Range(0, audioClips.Count);
        audioSource.PlayOneShot(audioClips[r]);
    }
}
