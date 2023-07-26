using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource audioSourceMuted;
    [SerializeField] AnimationClip animationClip;
    [SerializeField] AudioSource audioSource;
    //[SerializeField] SoundManager soundManager;

    void Start()
    {
        mixer.SetFloat("MyExposedParam", -80.0f);
        if (animationClip != null)
        {
            audioSource.PlayDelayed(animationClip.length);
        }
        else
        {
            audioSource.Play();
        }
    }

    public AudioSource GetAudioSource()
    {
        return audioSourceMuted;
    }
}
