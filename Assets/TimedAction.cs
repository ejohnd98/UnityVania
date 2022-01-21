using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedAction : MonoBehaviour
{
    public UnityEvent timedEvents;
    public float delay = 0.0f;
    float progress = 0.0f;
    int state = 0;
    public bool repeat = false;

    // Start is called before the first frame update
    void Start()
    {
        progress = 0.0f;
        state = 0;
    }

    public void DestroyThisObject(){
        ObjectHandler.DestroyObjects(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(state < 1){
            progress += Time.deltaTime;
            if(state == 0 && progress >= delay){
                state = 1;
                timedEvents.Invoke();
                if(repeat){
                    progress = 0.0f;
                    state = 0;
                }
            }
        }
    }
}
