using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeObject : MonoBehaviour
{
    public ShakePresets shakeType = ShakePresets.SmallEnemy;
    public float overrideLength = 1.0f;
    public float overrideMag = 0.1f;

    public void Shake(){
        ScreenShake.instance.StartShake(shakeType);
    }

    public void ShakeSpecific(){
        ScreenShake.instance.StartShake(overrideLength, overrideMag);
    }
}
