using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShakePresets{
        Player,
        SmallEnemy,
        MedEnemy,
        BigEnemy,
        Land
    }
public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }else{
            instance = this;
        }
    }

    public CameraFollow cameraFollow;
    float progress;
    public float length;
    public float magnitude;
    public bool isShaking = false;

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = Vector2.zero;
        if(isShaking){
            progress += Time.deltaTime;
            Vector2 rand = Random.insideUnitCircle;
            if(rand.magnitude < 0.75f){
                rand.Normalize();
            }

            offset = rand * magnitude * (1.0f - (progress / length));
            
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

    // for use with actions
    public void StartShortShake(float amount){
        StartShake(0.3f, amount);
    }
    public void StartMedShake(float amount){
        StartShake(0.8f, amount);
    }
    public void StartLongShake(float amount){
        StartShake(2.0f, amount);
    }

    public void StartShake(ShakePresets preset){
        switch(preset){
            case ShakePresets.Player:
                StartShake(0.2f, 0.2f);
                break;
            case ShakePresets.Land:
                StartShake(0.1f, 0.1f);
                break;
            case ShakePresets.SmallEnemy:
                StartShake(0.1f, 0.05f);
                break;
            case ShakePresets.MedEnemy:
                StartShake(0.2f, 0.1f);
                break;
            case ShakePresets.BigEnemy:
                StartShake(0.3f, 0.3f);
                break;
        }
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
