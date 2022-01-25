using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedObject : MonoBehaviour
{
    public GameObject timedObject;
    public float delay = 0.0f, length = 1.0f;
    float progress = 0.0f;
    int state = 0;
    public bool noEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        timedObject.SetActive(false);
        progress = 0.0f;
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(state < 2){
            progress += Time.deltaTime;
            if(state == 0 && progress >= delay){
                state = 1;
                timedObject.SetActive(true);
            }
            if(state == 1 && progress >= delay + length){
                state = 2;
                if(!noEnd)
                    timedObject.SetActive(false);
            }
        }
    }
}
