using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RandomAction : MonoBehaviour
{
    public UnityEvent events;
    public float minTime, maxTime;
    float chosenTime;
    float progress = 0.0f;
    public bool repeat = false;
    public bool active = true;

    public void DestroyThisObject(){
        ObjectHandler.DestroyObjects(this.gameObject);
    }

    void ChooseTime(){
        chosenTime = Random.Range(minTime, maxTime);
        progress = 0.0f;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(active){
            progress += Time.deltaTime;
            if(progress >= chosenTime){
                events.Invoke();
                active = false;
                if(repeat){
                    ChooseTime();
                }
            }
        }
    }
}
