using UnityEngine;
[CreateAssetMenu(fileName = "Game Setting Data", menuName = "Setting/SettingData")]
public class SettingsInformation : ScriptableObject
{

    [SerializeField] float MasterVolume, SFXVolume, MusicVolume;

    public float loadSettingData(string Type)
    {
        if (Type == "Master")
        {
            return MasterVolume;
        }
        else if (Type == "SFX")
        {
            return SFXVolume;

        }

        return MusicVolume;

    }
    public void SaveSettingData(string Type, float Amount)
    {
        if (Type == "Master")
        {
            MasterVolume = Amount;
        }
        else if (Type == "SFX")
        {
            SFXVolume = Amount;

        }
        else
        {
            MusicVolume = Amount;

        }

    }
}
