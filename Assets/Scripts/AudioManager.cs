using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);
        SFXVolume(sfxSlider.value);

        musicSlider.value = PlayerPrefs.GetFloat("musicVolume",1f);
        MusicVolume(musicSlider.value);


        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PLayMusic("MainMenu");
        }
    }

    public void PLayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SoundNotFound");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SoundNotFound");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    //set audio mixer volume
    public void MusicVolume(float volume)
    {
        if (volume <= 0.01)
        {
            audioMixer.SetFloat("Music", -80f);
            PlayerPrefs.SetFloat("musicVolume", -80f);
            return;
        }

        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    //set audio mixer volume
    public void SFXVolume(float volume)
    {
        if (volume <= 0.01)
        {
            audioMixer.SetFloat("SFX", -80f);
            PlayerPrefs.SetFloat("sfxVolume", -80f);
            return;
        }

        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
}