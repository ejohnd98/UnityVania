using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackFade : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    const float defaultFadeTime = 0.5f;
    float fadeTime = defaultFadeTime;
    float delayFadeIn = 0.5f;
    float progress = 0.0f;
    float delayProgress = 0.0f;
    Color spriteColor = Color.white;
    public bool fadingOut = false;
    public bool doneFading = true;

    public static BlackFade instance;

    private void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }else{
            instance = this;
        }
    }

    private void Start() {
        progress = fadingOut? 0.0f : 1.0f;
    }

    private void Update() {
        if(!doneFading){
            if(fadingOut && progress < 1.0f){
                progress = Mathf.Min(progress + Time.deltaTime / fadeTime, 1.0f);
                if(progress >= 0.995f){
                    progress = 1.0f;
                    doneFading = true;
                }
            }else if (progress > 0.0f){
                if(delayProgress < delayFadeIn){
                    delayProgress += Time.deltaTime;
                }else{
                    progress = Mathf.Max(progress - Time.deltaTime / fadeTime, 0.0f);
                }
                if(progress <= 0.005f){
                    progress = 0.0f;
                    doneFading = true;
                }
            }
            spriteColor.a = Easings.EaseInOutQuad(progress);
            spriteRenderer.color = spriteColor;
        }
    }

    public void FadeIn(float time = defaultFadeTime){
        delayProgress = 0.0f;
        fadeTime = time;
        progress = 1.0f;
        fadingOut = false;
        doneFading = false;
    }
    public void FadeOut(float time = defaultFadeTime){
        fadeTime = time;
        progress = 0.0f;
        fadingOut = true;
        doneFading = false;
    }

    public void SetOpacity(float a){
        progress = a;
    }

    public bool DoneFading(){
        return doneFading;
    }
}
