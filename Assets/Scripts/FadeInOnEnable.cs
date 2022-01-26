using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOnEnable : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color spriteColor = Color.white;
    bool started = false;
    public float fadeTime = 0.4f;
    public float progress = 0.0f;
    public bool flashOnEnable = true;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        progress = 0.0f;
        started = true;
        GetComponent<SpriteModifier>()?.FlashWhite(fadeTime);
    }

    private void Update() {
        if(started){
            if(progress < fadeTime){
            progress += Time.deltaTime;
            spriteColor.a = (progress/fadeTime);
            spriteRenderer.color = spriteColor;
            }else{
                started = false;
            }
        }
    }
}
