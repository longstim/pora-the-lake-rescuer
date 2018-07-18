using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class SoundPopupController : MonoBehaviour
{

    public GameObject toogleMusic;
    public GameObject toogleEffect;

    public GameObject scrollMusic;
    public GameObject scrollEffect;

    UserSoundConfig config;

    Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        config = UserSoundConfig.Load();

        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        for (int i = 0; i < musics.Length; i++)
        { 
            AudioSource audioSource = musics[i].GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.volume = config.MusicVolume;
                audioSource.mute = !config.MusicStatus;
            }
        }

        GameObject[] effects = GameObject.FindGameObjectsWithTag("SoundEffect");
        for (int i = 0; i < effects.Length; i++)
        {
            AudioSource audioSource = effects[i].GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.volume = config.SoundVolume;
                audioSource.mute = !config.SoundStatus;
            }
        }

        toogleMusic.GetComponent<Toggle>().isOn = config.MusicStatus;
        toogleEffect.GetComponent<Toggle>().isOn = config.SoundStatus;

        scrollEffect.GetComponent<Slider>().value = config.SoundVolume;
        scrollMusic.GetComponent<Slider>().value = config.MusicVolume;
         

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
        config.Save();
    }

	public void Press()
	{
		animator.SetTrigger ("Press");
	}

    public void OnChangeMusic(Single volume)
    {
        config.MusicVolume = volume;
        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        for (int i = 0; i < musics.Length; i++)
        {
            AudioSource audioSource = musics[i].GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.volume = config.MusicVolume;
            }
        }
    }

    public void OnChangeMusic(bool status)
    {
        config.MusicStatus = status;
        GameObject[] musics = GameObject.FindGameObjectsWithTag("Music");
        for (int i = 0; i < musics.Length; i++)
        {
            AudioSource audioSource = musics[i].GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.mute = !config.MusicStatus;
            }
        }
    }

    public void OnChangeEffect(Single volume)
    {
        config.SoundVolume = volume;
        GameObject[] effects = GameObject.FindGameObjectsWithTag("SoundEffect");
        for (int i = 0; i < effects.Length; i++)
        {
            AudioSource audioSource = effects[i].GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.volume = config.SoundVolume;
            }
        }
    }

    public void OnChangeEffect(bool status)
    {
        config.SoundStatus = status;
        GameObject[] effects = GameObject.FindGameObjectsWithTag("SoundEffect");
        for (int i = 0; i < effects.Length; i++)
        {
            AudioSource audioSource = effects[i].GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.mute = !config.SoundStatus;
            }
        }
    }
}
