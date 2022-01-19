using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    public float rotateSpeed = 1.0f;


    // Update is called once per frame
    void Update()
    {
        Vector3 angles = transform.localEulerAngles;
        angles.z += Time.deltaTime * rotateSpeed;
        transform.localEulerAngles = angles;
            
    }
}
