using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform spawnLocation;
    public bool discovered = false;
    public PortalHandler handler;

    public void ActivatePortal(){
        discovered = true;
        handler.StartSelection(this);
    }
}
