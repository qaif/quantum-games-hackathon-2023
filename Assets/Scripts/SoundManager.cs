using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager instance_ = null;
    public SoundManager Instance => instance_ ?? (instance_ = FindObjectOfType<SoundManager>());

    public AudioSource audioSource;

    [Header("Sound effects")]
    public AudioClip bet;
    public AudioClip call;
    public AudioClip card;
    public AudioClip click;
    public AudioClip gate;
    public AudioClip lose;
    public AudioClip measure;
    public AudioClip win;

    [Header("Music")]
    public AudioClip menu;
    public AudioClip gameplay;
    public AudioClip boss;

    public void Bet()
    {
        audioSource.PlayOneShot(bet);
    }

    public void Call()
    {
        audioSource.PlayOneShot(call);
    }

    public void Card()
    {
        audioSource.PlayOneShot(card);
    }

    public void Click()
    {
        audioSource.PlayOneShot(click);
    }

    public void Gate()
    {
        audioSource.PlayOneShot(gate);
    }

    public void Lose()
    {
        audioSource.PlayOneShot(lose);
    }

    public void Measure()
    {
        audioSource.PlayOneShot(measure);
    }

    public void Win()
    {
        audioSource.PlayOneShot(win);
    }
}
