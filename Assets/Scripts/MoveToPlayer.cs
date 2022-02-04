using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveToPlayer : MonoBehaviour
{
    public Vector3 finalScale;
    Vector3 startPos;
    Vector3 startLocalScale;
    Vector3 initPlayerPos;
    public Vector3 targetOffset = Vector3.zero;
    bool remainInPos = false;
    bool useCubicEaseIn = true;

    public UnityEvent finishEvent;

    public bool notStarted = true;
    public float lerpSpeed =1.0f;
    public bool onlyUseInitialPos = false;
    float progress = 0.0f;

    private void OnEnable() {
        StartAnim();
    }

    public void RemainStationary(){
        remainInPos = true;
    }

    public void StartAnim(){
        notStarted = false;
        progress = 0.0f;
        startPos = transform.position;
        startLocalScale = transform.localScale;
        initPlayerPos = PlayerController.instance.CenterOfPlayer;
    }

    // Update is called once per frame
    void Update()
    {

        if(!notStarted){
            progress += Time.deltaTime * lerpSpeed;

            Vector3 a = startPos;
            Vector3 b = remainInPos? a : (targetOffset + (onlyUseInitialPos? initPlayerPos : PlayerController.instance.CenterOfPlayer));
            transform.position = Vector3.Lerp(a, b, useCubicEaseIn? Easings.EaseInCubic(Mathf.Clamp01(progress)) : Easings.EaseInOutQuad(Mathf.Clamp01(progress)));
            
            Vector3 a2 = startLocalScale;
            Vector3 b2 = finalScale;
            transform.localScale = Vector3.Lerp(a2, b2, useCubicEaseIn? Easings.EaseInCubic(Mathf.Clamp01(progress)) : Easings.EaseInOutQuad(Mathf.Clamp01(progress)));
            

            if(progress >= 1.0){
                notStarted = true;
                finishEvent.Invoke();
            }
        }
    }
}
