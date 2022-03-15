using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModifier : MonoBehaviour
{
    CameraFollow camFollow;

    // Start is called before the first frame update
    void Start(){
        camFollow = GameObject.FindObjectOfType<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        camFollow.followOverride = transform;
    }

    private void OnTriggerExit2D(Collider2D other){
        if(camFollow.followOverride == transform){
            camFollow.followOverride = null;
        }
    }
}
