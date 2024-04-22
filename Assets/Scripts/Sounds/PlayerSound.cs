using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource singleShotAudioSource;
    [SerializeField] private AudioSource thrustAudioSource;
    [SerializeField] private AudioSource movingAudioSource;
    
    public void PlayShootSound(AudioClip audioClip)
    {
        singleShotAudioSource.PlayOneShot(audioClip);
    }
    public void PlayThrustAudio()
    {
        if(thrustAudioSource.isPlaying){return;}
        thrustAudioSource.Play();
    }
    public void StopThrustAudio()
    {
        if(!thrustAudioSource.isPlaying){return;}
        thrustAudioSource.Stop();
    }
    public void PlayMovingAudio()
    {
        if(movingAudioSource.isPlaying){return;}
        movingAudioSource.Play();
    }
    public void StopMovingAudio()
    {
        if(!movingAudioSource.isPlaying){return;}
        movingAudioSource.Stop();
    }
}
