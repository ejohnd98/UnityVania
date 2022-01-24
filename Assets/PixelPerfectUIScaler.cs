using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class PixelPerfectUIScaler : MonoBehaviour
{
    public PixelPerfectCamera ppc;
    CanvasScaler canvas;
    public int ratio;

    private void Awake() {
        canvas = GetComponent<CanvasScaler>();
    }

    void LateUpdate()
    {
        UpdateScale();
    }

    void UpdateScale(){
        canvas.scaleFactor = ppc.pixelRatio;
        ratio = ppc.pixelRatio;
    }
}
