using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStateHandler : MonoBehaviour
{
    public GameObject[] onDestruction, onDeactivation, onActivation;
    public Transform creationPoint;

    private void Awake() {
        if(creationPoint == null){
            creationPoint = transform;
        }
        
    }

    public void CreateObjects(StateTypes newState){
        switch(newState){
            case StateTypes.Activate:
                foreach(GameObject obj in onActivation){
                    GameObject newObj = Instantiate(obj, creationPoint.position, creationPoint.rotation);
                }
                break;
            case StateTypes.Deactivate:
                foreach(GameObject obj in onDeactivation){
                    GameObject newObj = Instantiate(obj, creationPoint.position, creationPoint.rotation);
                }
                break;
            case StateTypes.Destroy:
                foreach(GameObject obj in onDestruction){
                    GameObject newObj = Instantiate(obj, creationPoint.position, creationPoint.rotation);
                }
                break;
            default:
            break;
        }
        
    }
}
