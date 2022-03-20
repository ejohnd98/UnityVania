using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CameraFollow : MonoBehaviour
{
    public Transform toFollow;
    public Transform followOverride;
    public Vector3 offset;
    public PixelPerfectCamera ppc;
    public bool followX, followY;
    public Vector3 desiredPos;

    public float minXFollow, minYFollowAbove, minYFollowBelow;

    float pixelSnap;
    int maxPixelsPerStep = 2;

    private void Start() {
        pixelSnap = 1.0f / ppc.assetsPPU;
        transform.position = toFollow.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(followOverride != null){
            desiredPos = followOverride.position + offset;
        }else{
            UpdatePos();
        }
        SmoothFollowPixelPerfect();
    }

    void UpdatePos(){
        Vector3 newPos = transform.position;

        if(followX){
            float currentX = newPos.x;
            float targetX = toFollow.position.x;

            if(currentX < targetX - minXFollow){
                currentX = targetX - minXFollow;
            }
            if(currentX > targetX + minXFollow){
                currentX = targetX + minXFollow;
            }

            newPos.x = currentX;
        }
        if(followY){
            float currentY = newPos.y;
            float targetY = toFollow.position.y;

            if(currentY < targetY - minYFollowAbove){
                currentY = targetY - minYFollowAbove;
            }
            else if(currentY > targetY + minYFollowBelow){
                currentY = targetY + minYFollowBelow;
            }

            newPos.y = currentY;
        }

        desiredPos = newPos + offset;
    }

    public int xmod, ymod;
    
    void SmoothFollowPixelPerfect(){
        Vector3 target = ppc.RoundToPixel(desiredPos);
        Vector3 current = ppc.RoundToPixel(transform.position);
        int xMod = (int)Mathf.Abs((target.x - current.x)*0.75f);
        xMod = Mathf.Min(2 + xMod, 99);
        int yMod = (int)Mathf.Abs((target.y - current.y)*0.75f);
        yMod = Mathf.Min(2 + yMod, 99);

        xmod = xMod; ymod = yMod;

        for(int i = 0; current.x < target.x && i < xMod; i++){
            current.x += pixelSnap;
        }
        for(int i = 0; current.x > target.x && i < xMod; i++){
            current.x -= pixelSnap;
        }
        for(int i = 0; current.y < target.y && i < yMod; i++){
            current.y += pixelSnap;
        }
        for(int i = 0; current.y > target.y && i < yMod; i++){
            current.y -= pixelSnap;
        }
        transform.position = ppc.RoundToPixel(current);
    }
}
