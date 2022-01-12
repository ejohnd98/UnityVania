using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    public GameObject colliderObject;
    public bool isOpen = true;

    // Start is called before the first frame update
    void Awake(){
        animator = GetComponentInChildren<Animator>();
    }

    public void SetOpen(bool newState){
        isOpen = newState;
        colliderObject.SetActive(!newState);
        animator.SetBool("isOpen", isOpen);
    }
}
