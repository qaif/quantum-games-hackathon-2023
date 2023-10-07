using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager instance_ = null;
    public static SoundManager Instance => instance_ ?? (instance_ = FindObjectOfType<SoundManager>());

    public AudioSource musicSource;
    public AudioSource soundEffectsSource;

    [Header("Sound effects")]
    public AudioClip money;
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

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Money()
    {
        soundEffectsSource.PlayOneShot(money);
    }

    public void Call()
    {
        soundEffectsSource.PlayOneShot(call);
    }

    public void Card()
    {
        soundEffectsSource.PlayOneShot(card);
    }

    public void Click()
    {
        soundEffectsSource.PlayOneShot(click);
    }

    public void Gate()
    {
        soundEffectsSource.PlayOneShot(gate);
    }

    public void Lose()
    {
        soundEffectsSource.PlayOneShot(lose);
    }

    public void Measure()
    {
        soundEffectsSource.PlayOneShot(measure);
    }

    public void Win()
    {
        soundEffectsSource.PlayOneShot(win);
    }

    // Music
    void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void Menu()
    {
        PlayMusic(menu);
    }

    public void Level()
    {
        PlayMusic(gameplay);
    }

    public void Boss()
    {
        PlayMusic(boss);
    }

}
