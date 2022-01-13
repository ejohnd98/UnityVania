using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlatform : MonoBehaviour
{
    public float height;
    public float progress = 0.0f;
    public float timeToMove = 10.0f;
    Vector3 start, end;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        end = start + new Vector3(0.0f, height, 0.0f);
    }

    // Update is called once per frame
    void Update(){
        end = start + new Vector3(0.0f, height, 0.0f);
        progress += Time.deltaTime;
        if(progress <= timeToMove/2.0f){
            transform.position = Vector3.Slerp(start, end, progress/(timeToMove/2.0f));
        }
        if(progress >= timeToMove/2.0f){
            transform.position = Vector3.Slerp(end, start, (progress - timeToMove/2.0f)/(timeToMove/2.0f));
        }
        if(progress >= timeToMove){
            progress = 0.0f;
        }
        
    }
}
