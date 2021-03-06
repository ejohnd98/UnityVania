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
    DamageObject damageObject;
    bool isEnemy;

    private void Start() {
        controller = GetComponentInParent<PlatformControllerBase>();
        isEnemy = GetComponentInParent<PlayerController>() == null;
    }

    public void DetectHit(){
        if(currentlyColliding){
            if(otherCol == null || (isEnemy && otherCol.GetComponent<playerAttack>() == null)){
                return;
            }
            otherController = otherCol.transform.GetComponentInParent<PlatformControllerBase>();
            damageObject = otherCol.GetComponent<DamageObject>();
            
            if(otherController!=null && !otherController.SimActive() && damageObject == null){
                return;
            }

            hitEvents.Invoke();
            if(controller != null && otherCol != null){
                controller.ReceiveHit(otherCol.gameObject);
            }else if(damageObject != null){
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
