using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontPointSetter : MonoBehaviour
{
    public Font[] fonts;
    // Start is called before the first frame update
    void Awake()
    {
        foreach(Font fnt in fonts){
            fnt.material.mainTexture.filterMode = FilterMode.Point;
        }
    }
}
