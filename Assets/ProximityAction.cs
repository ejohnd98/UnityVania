using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProximityAction : MonoBehaviour
{
    public UnityEvent actionList;

    private void OnTriggerEnter2D(Collider2D other){
        actionList.Invoke();
    }
}
