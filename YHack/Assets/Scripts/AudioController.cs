using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] 
    AudioSource musicSource;
    [SerializeField]
    AudioSource SFXSource;

    public AudioClip bg;
    public AudioClip destroyed;
    public AudioClip engine;
    public AudioClip enginestart;
    public AudioClip ffUP;
    public AudioClip land;
    public AudioClip pushandpull;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicSource.clip = bg;
        musicSource.volume = (float)0.35; 
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
