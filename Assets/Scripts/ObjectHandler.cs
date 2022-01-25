using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateTypes{
    Activate,
    Deactivate,
    Destroy
}

public class ObjectHandler : MonoBehaviour
{

    static public void SetObjectActive(GameObject obj, bool setActive){
        obj.GetComponent<ObjectStateHandler>()?.CreateObjects(setActive? StateTypes.Activate : StateTypes.Deactivate);
        obj.SetActive(setActive);
    }

    static public void DestroyObjects(GameObject obj){
        obj.GetComponent<ObjectStateHandler>()?.CreateObjects(StateTypes.Destroy);
        Destroy(obj);
    }

}
