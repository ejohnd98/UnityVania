using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraFollow : MonoBehaviour
{
    public Transform toFollow;
    public PixelPerfectCamera ppc;
    public bool followX, followY;

    // Update is called once per frame
    void EarlyUpdate()
    {
        UpdatePos();
    }
    void FixedUpdate()
    {
        UpdatePos();
    }
    void LateUpdate()
    {
        UpdatePos();
    }

    void UpdatePos(){
        Vector3 newPos = transform.position;
        if(followX){
            newPos.x = toFollow.position.x;
        }
        if(followY){
            newPos.y = toFollow.position.y;
        }
        //transform.position = newPos;
        transform.position = ppc.RoundToPixel(newPos);
    }
}
