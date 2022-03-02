using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIStatic
{
    public static float musicVol;
    public static float soundEffVol;
        
    public static void SaveMusicSettings (float musicVol)
    {
        PlayerPrefs.SetFloat("musicVol", musicVol);
    }
    public static void SaveSoundEffSettings (float soundEffVol)
    {
        PlayerPrefs.SetFloat("soundEffVol", soundEffVol);
    }
    public static float GetMusicSettings()
    {
        musicVol = PlayerPrefs.GetFloat("musicVol");
        return musicVol;
    }
    public static float GetSoundEffSettings()
    {
        soundEffVol = PlayerPrefs.GetFloat("soundEffVol");
        return soundEffVol;
    }
}
