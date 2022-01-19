using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimScript : MonoBehaviour
{
    public Vector3[] animationSpots, animationScales, animationRotations;
    public bool useSpots = true, useScales = false, useRotations = false;
    Vector3 localStartPos;
    Vector3 localStartScale;
    Vector3 localStartRot;
    public float[] timeAtEachSpot;
    public bool notStarted = true;
    public bool debugStart = false;
    public bool startOnEnable = false;

    float progress = 0.0f;
    int nextState = 0;

    private void Awake() {
        localStartPos = transform.localPosition;
        localStartScale = transform.localScale;
        localStartRot = transform.localEulerAngles;
    }

    private void Start() {
        if(startOnEnable){
            StartAnim();
        }
    }

    public void StartAnim(){
        notStarted = false;
        progress = 0.0f;
        nextState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(debugStart){
            debugStart = false;
            StartAnim();
        }

        if(!notStarted){
            progress += Time.deltaTime;
            if(useSpots){
                Vector3 a = localStartPos + ((nextState == 0)? Vector3.zero : animationSpots[nextState-1]);
                Vector3 b = localStartPos + animationSpots[nextState];
                transform.localPosition = Vector3.Lerp(a, b, Easings.EaseInOutQuad(Mathf.Clamp01(progress)));
            }
            if(useScales){
                Vector3 a = ((nextState == 0)? localStartScale : animationScales[nextState-1]);
                Vector3 b = animationScales[nextState];
                transform.localScale = Vector3.Lerp(a, b, Easings.EaseInOutQuad(Mathf.Clamp01(progress)));
            }
            if(useRotations){
                Vector3 a = localStartRot + ((nextState == 0)? Vector3.zero : animationSpots[nextState-1]);
                Vector3 b = localStartRot + animationSpots[nextState];
                transform.localEulerAngles = Vector3.Lerp(a, b, Easings.EaseInOutQuad(Mathf.Clamp01(progress)));
            }
            

            if(progress >= 1.0 + timeAtEachSpot[nextState]){
                nextState++;
                progress = 0.0f;
                if(nextState >= Mathf.Max(animationSpots.Length, animationScales.Length, animationRotations.Length)){
                    nextState = 0;
                    progress = 0.0f;
                    notStarted = true;
                }
            }
        }
    }
}
