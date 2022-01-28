using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    public float rotateSpeed = 1.0f;
    public bool useGlobal = false;


    // Update is called once per frame
    void Update()
    {
        if(useGlobal){
            Vector3 angles = transform.eulerAngles;
            angles.z = GlobalRotate.instance.rotationZ;
            transform.eulerAngles = angles;
        }else{
            Vector3 angles = transform.localEulerAngles;
            angles.z += Time.deltaTime * rotateSpeed;
            transform.localEulerAngles = angles;
        }
    }
}
