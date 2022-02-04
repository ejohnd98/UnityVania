using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easings
{

    static public float EaseInOutQuad(float x) {
        return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    }

    static public float EaseInCubic(float x){
        return x * x * x;
    }
}
