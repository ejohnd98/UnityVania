using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitDetector : MonoBehaviour
{
    public UnityEvent hitEvents;
    PlayerController pc;
    EnemyController ec;
    bool currentlyColliding = false;
    Collider2D otherCol;

    private void Start() {
        pc = GetComponentInParent<PlayerController>();
        ec = GetComponentInParent<EnemyController>();
    }

    public void DetectHit(){
        if(currentlyColliding){
            hitEvents.Invoke();
            if(pc != null){
                pc.ReceiveHit(otherCol.gameObject);
            }
            if(ec != null){
                ec.ReceiveHit(otherCol.gameObject);
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
    }
}
