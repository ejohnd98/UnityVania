using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalHandler : MonoBehaviour
{
    public GameObject selectionUI;
    GameObject effectsObject;
    public GameObject portalSpriteObj;
    public GameObject effectsPrefab;
    public float movePlayerDelay = 1.0f;
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
    public PopUpSystem popUp;
    public Transform cam;

    //debug
    public bool moveUp, moveDown, waitingOnPrompt = false, ignoreInputAfterPromptFlag = false;

    private void Update() {
        if(moveUp){
            moveUp = false;
            MoveSelector(-1);
        }
        if(moveDown){
            moveDown = false;
            MoveSelector(1);
        }
        if(isSelecting && !waitingOnPrompt){
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

        if(ignoreInputAfterPromptFlag){
            ignoreInputAfterPromptFlag = false;
            waitingOnPrompt = false;
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
        SoundSystem.instance.PlaySound("ui_move");
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
        if(startIndex != selectionIndex){
            string prompt = "Travel to " + portals[selectionIndex].name + "?";
            popUp.PromptChoice(ChoiceType.Binary, StartPortalMove, prompt, "No", "Yes", true, 0.5f);
            waitingOnPrompt = true;
        }else{
            startIndex = -1;
            player.StopInputs(false);
            StopSelecting();
            StartCoroutine(PortalCooldown());
        }
    }

    public void StartPortalMove(Result choice){
        if(choice.choiceResult){
            isSelecting = false;
            selectionUI.SetActive(false);
            coolingDown = true;
            effectsObject = Instantiate(effectsPrefab, portals[startIndex].transform.position, Quaternion.identity);
            StartCoroutine(WaitForEffects());
            StartCoroutine(MovePlayer());
            SoundSystem.instance.PlaySound("portalSound");
        }        
        ignoreInputAfterPromptFlag = true;
    }

    IEnumerator MovePlayer(){
        yield return new WaitForSeconds(movePlayerDelay);
        Vector3 moveOffset = player.transform.position - portals[startIndex].transform.position;
        Vector3 cameraOffset = cam.position - portals[startIndex].transform.position;
        player.transform.position = portals[selectionIndex].transform.position + moveOffset + Vector3.up*0.005f;
        cam.position =  portals[selectionIndex].transform.position + cameraOffset + Vector3.up*0.005f;

        effectsObject.transform.position = portals[selectionIndex].transform.position;
    }

    IEnumerator WaitForEffects(){
        yield return new WaitWhile(() => effectsObject != null);
        startIndex = -1;
        player.StopInputs(false);
        StartCoroutine(PortalCooldown());
    }

    public void StopSelecting(){
        isSelecting = false;
        startIndex = -1;
        selectionUI.SetActive(false);
        coolingDown = true;
        waitingOnPrompt = false;
        player.StopInputs(false);
        StartCoroutine(PortalCooldown());
    }

    public void StartSelection(Portal currentPortal){
        if(coolingDown){
            return;
        }
        
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
        ignoreInputAfterPromptFlag = false;
        waitingOnPrompt = false;
    }

    IEnumerator PortalCooldown(){
        yield return new WaitForSeconds(cooldown);
        coolingDown = false;
    }
}
