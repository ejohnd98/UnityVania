using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomDestroy : MonoBehaviour
{
    public UnityEvent destroyEvents;

    public void StartDestroy(){
        destroyEvents.Invoke();
    }
}
