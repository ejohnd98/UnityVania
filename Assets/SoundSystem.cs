using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSystem : MonoBehaviour
{
    AudioSource musicPlayer;
    public GameObject soundEffectPrefab;
    public float musicVolume = 1.0f;

    public Areas musicArea = Areas.None;
    public AudioClip[] music;
    AudioClip nextMusic;

    bool fadingOut = false;
    public float fadeOutTime = 2.0f;
    float fadeOutCounter = 0.0f;

    void Start(){
        musicPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
        if(fadingOut && musicPlayer.volume > 0.0){
            musicPlayer.volume = easeInOutCubic(1.0f - fadeOutCounter/fadeOutTime);
            fadeOutCounter += Time.deltaTime;
        }
        if(!fadingOut && musicPlayer.volume < musicVolume){
            musicPlayer.volume = easeInOutCubic(1.0f - fadeOutCounter/fadeOutTime);
            fadeOutCounter -= Time.deltaTime;
        }
    }

    public void ChangeMusic(Areas area){
        musicArea = area;
        AudioClip newMusic = music[(int)area];
        nextMusic = newMusic;
        if(musicPlayer.isPlaying && fadingOut && nextMusic == musicPlayer.clip){
            fadingOut = false;
        }else{
            StartCoroutine(TransitionMusic());
        }
        
    }

    IEnumerator TransitionMusic(){
        fadingOut = true;
        yield return new WaitUntil(() => (musicPlayer.volume <= 0.001f || !musicPlayer.isPlaying));
        fadingOut = false;
        fadeOutCounter = 0.0f;
        musicPlayer.Stop();
        if(nextMusic != null){
            musicPlayer.clip = nextMusic;
            musicPlayer.volume = musicVolume;
            musicPlayer.Play();
        }
    }

    float easeInOutCubic(float x) {
        return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
    }
}
