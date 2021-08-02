using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform toFollow;
    public bool followX, followY;

    // Update is called once per frame
    void EarlyUpdate()
    {
        UpdatePos();
    }
    void FixexUpdate()
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
        transform.position = newPos;
    }
}
