using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem instance;

    AudioSource musicPlayer;
    public GameObject soundEffectPrefab;
    public float overallVolume = 1.0f;
    public float musicVolume = 0.5f;
    public float sfxVolume = 0.8f;
    public bool playPlaceholder = false;

    public Areas musicArea = Areas.None;
    public AudioClip[] music;
    public AudioClip[] sfxList;
    public string[] sfxWithVariance;
    public int[] sfxVarianceCount;
    private int[] sfxLastVariantIndex;
    Dictionary<string, AudioClip> sfx;

    AudioClip nextMusic;

    bool fadingOut = false;
    public float fadeOutTime = 2.0f;
    float fadeOutCounter = 0.0f;

    float currentMusicVol = 1.0f;

    private void Awake(){
        Debug.Log("Awake");
        if (instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        }else{
            instance = this;
        }

        if(sfx == null){
            sfx = new Dictionary<string, AudioClip>();
            sfxLastVariantIndex = new int[sfxWithVariance.Length];
            foreach(AudioClip clip in sfxList){
                sfx.Add(clip.name, clip);
                //Debug.Log("added " + clip.name);
            }
        }

        musicPlayer = GetComponent<AudioSource>();
    }

    private void UpdateSoundSettings(){
        overallVolume = SettingsObject.instance.globalVolume;
        musicVolume = SettingsObject.instance.musicVolume;
        sfxVolume = SettingsObject.instance.sfxVolume;
    }

    // Update is called once per frame
    void Update(){
        UpdateSoundSettings();
        if(fadingOut && currentMusicVol > 0.0){
            currentMusicVol = 1.0f * easeInOutCubic(1.0f - fadeOutCounter/fadeOutTime);
            fadeOutCounter += Time.deltaTime;
        }
        if(!fadingOut && currentMusicVol < 1.0f){
            currentMusicVol = 1.0f * easeInOutCubic(1.0f - fadeOutCounter/fadeOutTime);
            fadeOutCounter -= Time.deltaTime;
        }
        musicPlayer.volume = currentMusicVol * musicVolume* overallVolume;
    }

    public void PlaySound(string sndName){
        int varianceIndex = GetSoundVarianceIndex(sndName);
        if(varianceIndex >= 0){
            int rand = Random.Range(0, sfxVarianceCount[varianceIndex]);
            if(rand == sfxLastVariantIndex[varianceIndex]){
                rand = (rand + 1) % sfxVarianceCount[varianceIndex];
            }
            sfxLastVariantIndex[varianceIndex] = rand;
            sndName += (rand+1).ToString();
            //Debug.Log("chosen variant: " + sndName);
        }
        
        AudioClip clip;
        if (sfx.ContainsKey(sndName)){
            clip = sfx[sndName];
        }else{
            if(playPlaceholder){
                clip = sfx["placeholder"];
            }else{
                return;
            }
        }
        GameObject sndObj = Instantiate(soundEffectPrefab, transform);
        AudioSource newSrc = sndObj.GetComponent<AudioSource>();
        newSrc.volume = sfxVolume * overallVolume;
        newSrc.clip = clip;
    }

    public int GetSoundVarianceIndex(string snd){
        for(int i = 0; i < sfxWithVariance.Length; i++){
            if(sfxWithVariance[i].Equals(snd)){
                return i;
            }
        }
        return -1;
    }

    public void ChangeMusic(Areas area){
        musicArea = area;
        AudioClip newMusic = music[(int)area];
        nextMusic = newMusic;
        if(musicPlayer != null && musicPlayer.isPlaying && fadingOut && nextMusic == musicPlayer.clip){
            fadingOut = false;
        }else{
            StartCoroutine(TransitionMusic());
        }
        
    }

    public void PlayGameOver(){
        fadeOutTime = 0.5f;
        ChangeMusic(sfx["game over"]);
        musicPlayer.loop = false;
    }

    public void StopMusic(){
        ChangeMusic((AudioClip)null);
    }

    public void ChangeMusic(AudioClip newMusic){
        nextMusic = newMusic;
        if(musicPlayer.isPlaying && fadingOut && nextMusic == musicPlayer.clip){
            fadingOut = false;
        }else{
            StartCoroutine(TransitionMusic());
        }
        
    }

    IEnumerator TransitionMusic(){
        fadingOut = true;
        yield return new WaitUntil(() => (currentMusicVol <= 0.001f || !musicPlayer.isPlaying));
        fadingOut = false;
        fadeOutCounter = 0.0f;
        musicPlayer.Stop();
        if(nextMusic != null){
            musicPlayer.clip = nextMusic;
            currentMusicVol = 1.0f;
            musicPlayer.Play();
        }
    }

    float easeInOutCubic(float x) {
        return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }
}
