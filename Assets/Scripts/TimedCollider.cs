using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedCollider : MonoBehaviour
{
    public Collider2D timedCollider;
    public float delay = 0.0f, length = 1.0f;
    float progress = 0.0f;
    int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        timedCollider.enabled = false;
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
                timedCollider.enabled = true;
            }
            if(state == 1 && progress >= delay + length){
                state = 2;
                timedCollider.enabled = false;
            }
        }
    }
}
