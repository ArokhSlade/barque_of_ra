using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private string startingTitle;

    [SerializeField]
    private string boatSound;

    [SerializeField]
    private List<Sound> SoundEffects;

    [SerializeField]
    private List<Sound> BackgroundMusic;

    [SerializeField] float pauseTime = 0.5f;


    [SerializeField] AudioMixerGroup masterChannel;
    [SerializeField] AudioMixerGroup sfxChannel;
    [SerializeField] AudioMixerGroup musicChannel;

    Dictionary<string, Sound> musicDict = new();


    public static AudioManager Instance;


    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitBackgroundMusic();
        InitSoundEffects();    
        
    }

    private void InitBackgroundMusic()
    {
        foreach (Sound sound in BackgroundMusic)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.outputAudioMixerGroup = musicChannel;

            sound.source.volume = sound.volume;

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.playOnAwake = sound.playOnAwake;
            sound.source.loop = sound.loop;

            musicDict[sound.name] = sound;
        }
    }

    private void InitSoundEffects()
    {
        foreach (Sound sound in SoundEffects)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.outputAudioMixerGroup = sfxChannel;

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.playOnAwake = sound.playOnAwake;
            sound.source.loop = sound.loop;
        }
    }
   
    public static float NormalizedToDB(float normalizedVolume)
    {
        const float minVolumeDB = -80f;

        if (Mathf.Clamp01(normalizedVolume) != normalizedVolume)
        {
            Debug.LogWarning($"expected normalizedVolume but got {normalizedVolume}");
        }
        float dB = 0f;

        normalizedVolume = Mathf.Clamp01(normalizedVolume);

        if (normalizedVolume == 0f)
        {
            dB = minVolumeDB;
        }
        else
        {
            dB = 20f * Mathf.Log10(normalizedVolume);
        }

        return dB;
    }

    public void PlayMusic(string title)
    {
        Sound SoundToPlay = BackgroundMusic.Find(SoundElement => SoundElement.name == title);

        if (SoundToPlay != null)
        {
            SoundToPlay.source.Play();
        }

        else
        {
            Debug.LogError("Music: " + title + " not found");
        }
    }

    public void PauseMusic(string title)
    {
        Sound song = musicDict[title];
        
        StartCoroutine(DecreaseValueOverTime(song));
    }

    IEnumerator DecreaseValueOverTime(Sound song)
    {
        float decreaseRate = song.source.volume / pauseTime;
        while (song.source.volume > 0)
        {
            song.source.volume -= decreaseRate * Time.deltaTime;

            if (song.source.volume < 0)
            {
                song.source.volume = 0;
            }
            yield return null;
        }

        song.source.Pause();
    }


    public void UnpauseMusic(string title)
    {
        Sound song = musicDict[title];

        StartCoroutine(IncreaseValueOverTime(song));
    }

    IEnumerator IncreaseValueOverTime(Sound song)
    {
        float rate = song.volume / pauseTime;
        while (song.source.volume < song.volume)
        {
            song.source.volume += rate * Time.deltaTime;

            if (song.source.volume > 0)
            {
                song.source.volume = song.volume;
            }
            yield return null;
        }

        song.source.UnPause();
    }



    public void StopMusic(string soundEffect)
    {
        Sound SoundToPlay = BackgroundMusic.Find(SoundElement => SoundElement.name == soundEffect);

        if (SoundToPlay != null)
        {
            SoundToPlay.source.Stop();
        }

        else
        {
            Debug.LogError("Music: " + soundEffect + " not found");
        }
    }

    public void PlaySound(string soundEffect)
    {
        Sound SoundToPlay = SoundEffects.Find(SoundElement => SoundElement.name == soundEffect);

        if (SoundToPlay != null)
        {
            SoundToPlay.source.Play();
        }

        else
        {
            Debug.LogError("Sound Effect: " + soundEffect + " not found");
        }
    }

    public void StopSoundEffect(string soundEffect)
    {
        Sound SoundToPlay = SoundEffects.Find(SoundElement => SoundElement.name == soundEffect);

        if (SoundToPlay != null)
        {
            SoundToPlay.source.Stop();
        }

        else
        {
            Debug.LogError("Sound Effect: " + soundEffect + " not found");
        }
    }

    public void ChangeVolume(AudioMixerGroup channel, string parameter, float normalizedValue)
    {
        float dBValue = NormalizedToDB(normalizedValue);
        channel.audioMixer.SetFloat(parameter, dBValue);
    }

    public void ChangeMasterVolume(float normalizedValue)
    {
        ChangeVolume(masterChannel, "MasterVolume", normalizedValue);
    }

    public void ChangeMusicVolume(float normalizedValue)
    {
        ChangeVolume(musicChannel, "MusicVolume", normalizedValue);
    }
    public void ChangeSFXVolume(float normalizedValue)
    {
        ChangeVolume(sfxChannel, "SFXVolume", normalizedValue);
    }

}
