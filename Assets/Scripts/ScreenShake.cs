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

    private void Start() {
        shakeSetting = SettingsObject.instance.screenShake;
    }

    public CameraFollow cameraFollow;
    float progress;
    public float length;
    public float magnitude;
    public float constantShakeAmount = 0.0f;
    public bool isShaking = false;
    public bool constantShaking = false;
    public float shakeSetting = 1.0f;

    // Update is called once per frame
    void Update(){

        Vector2 offset = Vector2.zero;
        if(isShaking || constantShaking){
            if(isShaking){
                progress += Time.deltaTime;
            }
            
            Vector2 rand = Random.insideUnitCircle;
            if(rand.magnitude < 0.75f){
                rand.Normalize();
            }
            float appliedMag = constantShakeAmount + (isShaking? (magnitude * (1.0f - (progress / length))) : 0);
            offset = rand * appliedMag;
            offset *= shakeSetting;
            
            if(progress >= length){
                isShaking = false;
                progress = 0.0f;
            }
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

    public void SetConstantShake(float mag){
        constantShakeAmount = mag;
        constantShaking = true;
    }

    public void ResetConstantShake(){
        constantShakeAmount = 0.0f;
        constantShaking = false;
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
        shakeSetting = SettingsObject.instance.screenShake;
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
