using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRandomizer : MonoBehaviour
{
    public float rangeLow = 0.8f, rangeHigh = 1.2f;

    private void Start() {
        GetComponent<Animator>().SetFloat("speedMod", Random.Range(rangeLow, rangeHigh));
    }
}
