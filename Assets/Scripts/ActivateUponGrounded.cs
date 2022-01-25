using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateUponGrounded : MonoBehaviour
{
    public SimplePhysicsObject phys;
    public GameObject obj;
    bool hasActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasActivated && phys.grounded){
            obj.SetActive(true);
            hasActivated = true;
        }
    }
}
