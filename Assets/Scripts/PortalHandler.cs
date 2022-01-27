using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalHandler : MonoBehaviour
{
    public GameObject selectionUI;
    public Transform selectorTransform;
    public Transform[] selectionTransforms;
    public Text menuText;
    public Text greyMenuText;
    public int startIndex = -1;
    public int selectionIndex;
    public int portalsDiscovered = 0;
    public bool isSelecting = false;
    float cooldown = 0.5f;
    bool coolingDown = false;

    public Portal[] portals;
    public PlayerController player;
    public InputHandler inputHandler;

    //debug
    public bool moveUp, moveDown;

    private void Update() {
        if(moveUp){
            moveUp = false;
            MoveSelector(-1);
        }
        if(moveDown){
            moveDown = false;
            MoveSelector(1);
        }
        if(isSelecting){
            if(inputHandler.v_axis_pressed){
                MoveSelector((int)Mathf.Sign(-inputHandler.v_axis));
            }
            if(inputHandler.attack_button_pressed){
                SelectPortal();
            }
            if(inputHandler.jump_button_pressed){
                StopSelecting();
            }
        }
    }

    void UpdateText(){
        menuText.text = "Destination:" + '\n';
        greyMenuText.text = "Destination:" + '\n';

        foreach(Portal portal in portals){
            if(portal.discovered){
                menuText.text += portal.gameObject.name + " -" + '\n';
                greyMenuText.text += " -" + '\n';
            }else{
                greyMenuText.text += " -" + '\n';
                menuText.text += " " + '\n';
            }
        }
    }

    public void MoveSelector(int change){
        selectionIndex += change;
        while(selectionIndex >= portals.Length){selectionIndex -= portals.Length;}
        while(selectionIndex < 0){selectionIndex += portals.Length;}

        int missesInRow = 0;
        while(!portals[selectionIndex].discovered && missesInRow <= portals.Length){
            missesInRow++;
            selectionIndex += (change / Mathf.Abs(change));
            while(selectionIndex >= portals.Length){selectionIndex -= portals.Length;}
            while(selectionIndex < 0){selectionIndex += portals.Length;}
        }
        selectorTransform.position = selectionTransforms[selectionIndex].position;
    }

    public void SelectPortal(){
        isSelecting = false;
        selectionUI.SetActive(false);
        coolingDown = true;
        if(startIndex != selectionIndex){
            player.transform.position = portals[selectionIndex].spawnLocation.position;
            SoundSystem.instance.PlaySound("portalTravelSound");
            //wait for animation/whatever to finish
        }
        startIndex = -1;
        player.StopInputs(false);
        StartCoroutine(PortalCooldown());
    }

    public void StopSelecting(){
        isSelecting = false;
        startIndex = -1;
        selectionUI.SetActive(false);
        coolingDown = true;
        player.StopInputs(false);
        StartCoroutine(PortalCooldown());
    }

    public void StartSelection(Portal currentPortal){
        if(coolingDown){
            return;
        }
        SoundSystem.instance.PlaySound("portalStartSound");
        UpdateText();
        //set index to current portal
        for(int i = 0; i < portals.Length; i++){
            if(portals[i] == currentPortal){
                selectionIndex = i;
                startIndex = i;
                break;
            }
        }
        selectorTransform.position = selectionTransforms[selectionIndex].position;

        player.StopInputs(true);
        isSelecting = true;
        selectionUI.SetActive(true);
    }

    IEnumerator PortalCooldown(){
        yield return new WaitForSeconds(cooldown);
        coolingDown = false;
    }
}
