using UnityEngine;
using System;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance;

    public AudioMixer mixer;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.group;
        }

       
    }

    public void LoadAudioPrefs()
    {
        float fxVol = Settings.Instance.soundFXVolume;
        float musVol = Settings.Instance.musicVolume;
        bool muted = Settings.Instance.muted;

        mixer.SetFloat("musicVolume", LinearToLog(musVol));
        mixer.SetFloat("fxVolume", LinearToLog(fxVol));
        
    }

    private void Start()
    {
        LoadAudioPrefs();
        Play("Background");
    }

    //Is designed to take a volume value between .0001 and 1 and convert to a decibel value (-80 to 0), this makes slider scale affect sound volume linearly.
    public float LinearToLog(float value)
    {
        return Mathf.Log10(value) * 20;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            Debug.LogWarning("sound with name " + name + " was not found!");
            return;
        }
        s.source.Play();
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("sound with name " + name + " was not found!");
            return;
        }
        s.source.PlayOneShot(s.clip);
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("sound with name " + name + " was not found!");
            return;
        }
        s.source.Stop();
    }

    public void SetMute(bool mute)
    {
        Settings.Instance.SetMute(mute);
        if (mute)
        {
            mixer.SetFloat("masterVolume", -80f);
        }
        else
        {
            mixer.SetFloat("masterVolume", 0f);
        }
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("sound with name " + name + " was not found!");
            return;
        }
        s.source.Pause();
    }

    //Stops all sounds from playing. bool param asks if it should stop sounds playing on loop as well (default : false).
    //WARNING: does not stop sounds started with "PlayOneShot"
    public void StopAll(bool loops)
    {
        foreach (Sound s in sounds)
        {
            if (loops || s.loop == false)
                s.source.Stop();
        }
    }
    public void StopAll()
    {
        StopAll(false);
    }

    

}
