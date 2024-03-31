using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    // Looping sounds
    [SerializeField] 
    AudioSource musicSource, engineSource, pullSource, pushSource, fieldSource;

    // One shot sounds
    public AudioClip bg, destroyed, ffUP, land, pushandpull, killedJunk;

    private bool playingEngine = false;
    public bool PlayEngine {
        set { ChangeEngine(value); }
        get { return playingEngine; }
    }

    private bool playingField = false;
    public bool PlayForcefield {
        set { ChangeField(value); }
        get { return playingField; }
    }

    private bool playingPull = false;
    public bool PlayPull {
        set { ChangePull(value); }
        get { return playingPull; }
    }

    private bool playingPush = false;
    public bool PlayPush {
        set { ChangePush(value); }
        get { return PlayPush; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicSource.clip = bg;
        musicSource.volume = (float)0.23; 
    }

    public void StartMusic() {
        musicSource.Play();
    }

    private void ChangeEngine(bool newState) {
        if(playingEngine == newState) {
            return;
        }

        playingEngine = newState;

        if(playingEngine) {
            engineSource.Play();
        }
        else {
            engineSource.Stop();
        }
    }

    private void ChangeField(bool newState) {
        if(playingField == newState) {
            return;
        }

        playingField = newState;

        if(playingField) {
            fieldSource.Play();
        }
        else {
            fieldSource.Stop();
        }
    }

    private void ChangePull(bool newState) {
        if(playingPull == newState) {
            return;
        }

        playingPull = newState;

        if(playingPull) {
            pullSource.Play();
        }
        else {
            pullSource.Stop();
        }
    }

    private void ChangePush(bool newState) {
        if(playingPush == newState) {
            return;
        }

        playingPush = newState;

        if(playingPush) {
            pushSource.Play();
        }
        else {
            pushSource.Stop();
        }
    }

    public void StopAll() {
        PlayEngine = false;
        PlayPull = false;
        PlayPush = false;
        PlayForcefield = false;
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }
}
