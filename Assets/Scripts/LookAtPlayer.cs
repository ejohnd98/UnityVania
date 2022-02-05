using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LookAtPlayer : MonoBehaviour
{
    bool isLooking = true;

    public void SetLooking(bool state){
        isLooking = state;
    }

    // Update is called once per frame
    void Update()
    {

        if(isLooking){
            Vector3 target = PlayerController.instance.CenterOfPlayer;
            target.z = transform.position.z;
            transform.right = (target - transform.position).normalized;
            
        }
    }
}
