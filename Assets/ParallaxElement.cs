using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxElement : MonoBehaviour
{
    public Transform cam;
    public Vector3 lastPos;
    public Vector3 distanceTraveled;
    public Vector3 startPos;
    public float factorX = 2.0f, factorY = 4.0f;

    public int pixelOffsetX, pixelOffsetY;
    
    float step  = 1.0f/16.0f;

    void Awake(){
        lastPos = cam.position;
        startPos = transform.localPosition;
    }

    void Update(){
        distanceTraveled += cam.position - lastPos;
        lastPos = cam.position;

        pixelOffsetX = -Mathf.RoundToInt(distanceTraveled.x / (step * factorX));
        pixelOffsetY = -Mathf.RoundToInt(distanceTraveled.y / (step * factorY));

        Vector3 newPos = startPos;
        newPos.x  += (pixelOffsetX * step);
        newPos.y  += (pixelOffsetY * step);

        transform.localPosition = newPos;
    }

}