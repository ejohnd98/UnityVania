using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsObject : MonoBehaviour
{
    public static SettingsObject instance;

    public float globalVolume, musicVolume, sfxVolume;
    public float screenShake;
    private bool ignoreSavedSettings = false;

    private float globalDef = 0.8f, musicDef = 0.6f, sfxDef = 0.7f, shakeDef = 1.0f;
    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }else{
            instance = this;
            LoadSettings();
        }
    }

    public void SaveSettings(){
        PlayerPrefs.SetFloat("globalVolume", globalVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.SetFloat("screenShake", screenShake);

        PlayerPrefs.Save();
    }

    public void LoadSettings(){
        if(PlayerPrefs.HasKey("globalVolume") && !ignoreSavedSettings){
            globalVolume = PlayerPrefs.GetFloat("globalVolume");
            musicVolume = PlayerPrefs.GetFloat("musicVolume");
            sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
            screenShake = PlayerPrefs.GetFloat("screenShake");
        }else{
            globalVolume = globalDef;
            musicVolume = musicDef;
            sfxVolume = sfxDef;
            screenShake = shakeDef;
        }
    }
}
