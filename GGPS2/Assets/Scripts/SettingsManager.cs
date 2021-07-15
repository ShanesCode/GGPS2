using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject parentMenu;

    public AudioMixer mixer;
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;

    // Start is called before the first frame update
    void Awake()
    {
        masterVolume.onValueChanged.AddListener(SetMasterVolume);
        musicVolume.onValueChanged.AddListener(SetMusicVolume);
        sfxVolume.onValueChanged.AddListener(SetSFXVolume);
    }

    // Start is called before the first frame update
    void Start()
    {
        masterVolume.value = PlayerPrefs.GetFloat("masterVolume", 0.5f);
        musicVolume.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        sfxVolume.value = PlayerPrefs.GetFloat("sfxVolume", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToParent()
    {
        parentMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("masterVolume", masterVolume.value);
        PlayerPrefs.SetFloat("musicVolume", musicVolume.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume.value);
    }

    public void SetMasterVolume(float sliderValue)
    {
        mixer.SetFloat("masterVolume", Mathf.Log10(sliderValue) * 30);
        PlayerPrefs.SetFloat("masterVolume", sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("musicVolume", Mathf.Log10(sliderValue) * 30);
        PlayerPrefs.SetFloat("musicVolume", sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        mixer.SetFloat("sfxVolume", Mathf.Log10(sliderValue) * 30);
        PlayerPrefs.SetFloat("sfxVolume", sliderValue);
    }
}
