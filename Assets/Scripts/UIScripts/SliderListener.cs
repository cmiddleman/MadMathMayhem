using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderListener : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicSlider;
    public Slider FXSlider;
    public Toggle muteToggle;

    private void Start()
    {
        musicSlider.value = Settings.Instance.musicVolume;
        FXSlider.value = Settings.Instance.soundFXVolume;
        muteToggle.isOn = Settings.Instance.muted;
    }
    public void SetMusicVolume(float value)
    {
        Settings.Instance.SetMusicVolume(value);
        mixer.SetFloat("musicVolume", AudioManager.Instance.LinearToLog(value));
    }

    public void SetFXVolume(float value)
    {
        Settings.Instance.SetFXVolume (value);
        mixer.SetFloat("fxVolume", AudioManager.Instance.LinearToLog(value));
    }

    public void SetMute(bool mute)
    {
        AudioManager.Instance.SetMute(mute);
    }

    
}
