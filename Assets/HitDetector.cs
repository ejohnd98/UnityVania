using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitDetector : MonoBehaviour
{
    public UnityEvent hitEvents;
    PlatformControllerBase controller;
    bool currentlyColliding = false;
    Collider2D otherCol;
    PlatformControllerBase otherController;

    private void Start() {
        controller = GetComponentInParent<PlatformControllerBase>();
    }

    public void DetectHit(){
        if(currentlyColliding){
            if(otherCol == null){
                return;
            }
            otherController = otherCol.transform.GetComponentInParent<PlatformControllerBase>();
            
            if(otherController!=null && !otherController.SimActive()){
                return;
            }

            hitEvents.Invoke();
            if(controller != null){
                controller.ReceiveHit(otherCol.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        currentlyColliding = true;
        otherCol = other;
        DetectHit();
    }

    private void OnTriggerExit2D(Collider2D other){
        currentlyColliding = false;
        otherCol = null;
        otherController = null;
    }
}
