using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRotate : MonoBehaviour
{
    public static GlobalRotate instance;
    public float rotationZ;
    public float rotateSpeed = 5.0f;

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }else{
            instance = this;
        }
    }


    // Update is called once per frame
    void Update(){
        Vector3 rot = transform.eulerAngles;
        rot.z += Time.deltaTime * rotateSpeed;
        rotationZ = rot.z;
        transform.eulerAngles = rot;
    }
}
