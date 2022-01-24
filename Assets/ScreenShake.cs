using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public CameraFollow cameraFollow;
    float progress;
    public float length;
    public float magnitude;
    public bool isShaking = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = Vector2.zero;
        if(isShaking){
            progress += Time.deltaTime;
            offset = Random.insideUnitCircle * magnitude * (1.0f - (progress / length));
            
            if(progress >= length){
                isShaking = false;
                progress = 0.0f;
            }
        }
        
        if(length > 0.0f){
            
        }else{
            length = 0.0f;
            offset = Vector2.zero;
        }
        cameraFollow.offset = offset;
    }

    public void StartShake(float seconds, float amount){
        if(isShaking){
            float currentMag = magnitude * (1.0f - (progress / length));
            float currentLength = length - progress;

            magnitude = Mathf.Max(amount, currentMag);
            length = Mathf.Max(seconds, currentLength);
            progress = 0.0f;
        }else{
            length = seconds;
            magnitude = amount;
            isShaking = true;
        }
        

    }
}
