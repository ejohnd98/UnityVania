using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteModifier : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Material initialMat;
    public Material whiteMat;
    public bool deathEffect = false;
    public bool createEffectAsSibling = false;
    public float fadeTime = 0.4f;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialMat = spriteRenderer.material;
    }

    public void FlashWhite(float time = 0.0875f){
        spriteRenderer.material = whiteMat;
        StartCoroutine(ResetMat(time));
    }

    IEnumerator ResetMat(float time){
        yield return new WaitForSeconds(time);
        spriteRenderer.material = initialMat;
    }

    public void CreateDeathSprite(){
        GameObject deathSprite = new GameObject("deathSprite");
        deathSprite.transform.position = spriteRenderer.transform.position;
        deathSprite.transform.rotation = spriteRenderer.transform.rotation;
        if(createEffectAsSibling){
            deathSprite.transform.parent = spriteRenderer.transform.parent;
            deathSprite.transform.localScale = spriteRenderer.transform.localScale;
        }else{
            deathSprite.transform.localScale = spriteRenderer.transform.lossyScale;
        }
        SpriteRenderer newRenderer = deathSprite.AddComponent<SpriteRenderer>();
        newRenderer.sprite = spriteRenderer.sprite;
        newRenderer.material = whiteMat;
        newRenderer.maskInteraction = spriteRenderer.maskInteraction;
        newRenderer.renderingLayerMask = spriteRenderer.renderingLayerMask;
        newRenderer.sortingOrder = spriteRenderer.sortingOrder;

        FadeAwayThenDelete fade = deathSprite.AddComponent<FadeAwayThenDelete>();
        fade.fadeTime = fadeTime;
    }
}
