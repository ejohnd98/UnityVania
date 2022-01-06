using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    public Areas correspondingArea = Areas.None;
    AreaController areaController;

    // Start is called before the first frame update
    void Start(){
        areaController = GameObject.FindObjectOfType<AreaController>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        areaController.UpdateArea(correspondingArea, gameObject);
    }
}
