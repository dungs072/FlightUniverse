using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour
{
    [SerializeField] private AudioClip riffleAudio;
    [SerializeField] private AudioSource audioSource;

    public void PlayRiffleShootSound()
    {
        audioSource.PlayOneShot(riffleAudio);
    }
}
