using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingAdjuster : MonoBehaviour
{

    [SerializeField] Slider MasterVolume, SoundEffectVolume, MusicVolume;
    [SerializeField] AudioManager audioManager;
    [SerializeField] SettingsInformation DefaultSetting;
    private void Awake()
    {

        MasterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);
        SoundEffectVolume.onValueChanged.AddListener(OnSFXVolumeChanged);
        MusicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);

        MasterVolume.value = DefaultSetting.loadSettingData("Master");
        SoundEffectVolume.value = DefaultSetting.loadSettingData("SFX");
        MusicVolume.value = DefaultSetting.loadSettingData("Music");

        audioManager = AudioManager.Instance;

    }
    void OnMasterVolumeChanged(float Value)
    {
        audioManager.ChangeMasterVolume(Value);
    }
    void OnSFXVolumeChanged(float Value)
    {
        audioManager.ChangeSFXVolume(Value);
    }
    void OnMusicVolumeChanged(float Value)
    {
        audioManager.ChangeMusicVolume(Value);

    }
}
