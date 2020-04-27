using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    select, pause, sneeze, cough, powerUp, refill, infect, lose
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager inst { get; private set; }

    public AudioSource musicSource;
    public AudioSource fxSource1;
    public AudioSource fxSource2; // cough
    public AudioSource fxSource3; // sneeze
    public AudioSource fxSource4; // inefect

    public AudioClip select;
    public AudioClip pause;
    public AudioClip sneeze;
    public AudioClip cough;
    public AudioClip powerUp;
    public AudioClip refill;
    public AudioClip infect;
    public AudioClip lose;

    void Start()
    {
        inst = this;
    }

    public void changeMusicVolume(float num)
    {
        musicSource.volume = num;
    }

    public void changeFXVolume(float num)
    {
        fxSource1.volume = num;
        fxSource2.volume = num;
        fxSource3.volume = num;
        fxSource4.volume = num;
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResetMusic()
    {
        musicSource.Stop();
        PlayMusic();
    }

    public void PlaySound(Sound clip)
    {
        if (clip == Sound.cough)
        {
            fxSource2.clip = GetClip(clip);
            fxSource2.Play();
        }
        else if (clip == Sound.sneeze)
        {
            fxSource3.clip = GetClip(clip);
            fxSource3.Play();
        }
        else if (clip == Sound.infect)
        {
            fxSource4.clip = GetClip(clip);
            fxSource4.Play();
        }
        else
        {
            fxSource1.clip = GetClip(clip);
            fxSource1.Play();
        }
    }

    private AudioClip GetClip(Sound clip)
    {
        if (clip == Sound.select)
        {
            return select;
        }
        else if (clip == Sound.pause)
        {
            return pause;
        }
        else if (clip == Sound.sneeze)
        {
            return sneeze;
        }
        else if (clip == Sound.cough)
        {
            return cough;
        }
        else if (clip == Sound.powerUp)
        {
            return powerUp;
        }
        else if (clip == Sound.refill)
        {
            return refill;
        }
        else if (clip == Sound.infect)
        {
            return infect;
        }
        else if (clip == Sound.lose)
        {
            return lose;
        }

        return select;
    }
}
