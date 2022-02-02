using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform spawnLocation;
    public GameObject discoveredObj;
    public bool discovered = false;
    public PortalHandler handler;

    private void Start() {
        if(discovered){
            discoveredObj.SetActive(true);
        }
    }

    public void SetState(bool state){
        discovered = state;
        discoveredObj.SetActive(state);
    }

    public void ActivatePortal(){
        if(!discovered){
            SoundSystem.instance.PlaySound("portalFound");
            discoveredObj.SetActive(true);
        }
        discovered = true;
        handler.StartSelection(this);
    }
}
