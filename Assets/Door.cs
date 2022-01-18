using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    public GameObject colliderObject;
    public bool isOpen = true;
    public bool remainClosedAfterBoss = false;

    // Start is called before the first frame update
    void Awake(){
        animator = GetComponentInChildren<Animator>();
    }

    public void SetOpen(bool newState, bool force = false){
        if(remainClosedAfterBoss && newState && !force){
            return;
        }
        isOpen = newState;
        ObjectHandler.SetObjectActive(colliderObject, !newState);
        animator.SetBool("isOpen", isOpen);
    }
}
