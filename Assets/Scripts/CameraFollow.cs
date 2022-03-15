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

    public float minXFollow, minYFollowAbove, minYFollowBelow;

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdatePos();
    }
    void LateUpdate()
    {
        //UpdatePos();
        Debug.DrawLine(transform.position + Vector3.up * minYFollowAbove, transform.position - Vector3.up * minYFollowBelow, Color.cyan);
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
        transform.position = ppc.RoundToPixel(newPos+offset);
    }
}
