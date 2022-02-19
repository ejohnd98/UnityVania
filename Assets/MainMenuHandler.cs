using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{

    public SaveSystemLoader loader;

    public GameObject selectionUI;
    public Transform selectorTransform;
    public Transform[] selectionTransforms;
    public int startIndex = 0;
    public int selectionIndex;
    public bool isSelecting = false;
    bool[] disabledSelections = {false, false, false, false, false};
    public bool waitingOnPrompt = false;
    public bool ignoreInputAfterPromptFlag = false;
    public bool creditsIgnoreInputFlag = false;

    public InputHandler inputHandler;
    public PopUpSystem popUp;
    public OptionsMenuHandler optionsMenu;

    public GameObject creditsObj;

    //debug
    public bool moveUp, moveDown;

    private void Start() {
        loader = FindObjectOfType<SaveSystemLoader>();
        StartSelection();
    }

    private void Update() {
        if(moveUp){
            moveUp = false;
            MoveSelector(-1);
        }
        if(moveDown){
            moveDown = false;
            MoveSelector(1);
        }
        if(isSelecting && !waitingOnPrompt && !optionsMenu.isSelecting && !creditsObj.activeSelf){
            if(inputHandler.v_axis_pressed){
                MoveSelector((int)Mathf.Sign(-inputHandler.v_axis));
            }
            if(inputHandler.attack_button_pressed){
                SelectOption();
            }
        }
        if(ignoreInputAfterPromptFlag){
            ignoreInputAfterPromptFlag = false;
            waitingOnPrompt = false;
        }
        if(creditsObj.activeSelf && inputHandler.v_axis_pressed || inputHandler.h_axis_pressed || inputHandler.attack_button_pressed || inputHandler.jump_button_pressed){
            if(creditsIgnoreInputFlag){
                creditsIgnoreInputFlag = false; 
            }else{
                creditsObj.SetActive(false);
            }
        }
    }

    public void MoveSelector(int change){
        selectionIndex += change;
        while(selectionIndex >= selectionTransforms.Length){selectionIndex -= selectionTransforms.Length;}
        while(selectionIndex < 0){selectionIndex += selectionTransforms.Length;}
        
        int missesInRow = 0;
        while(disabledSelections[selectionIndex] && missesInRow <= selectionTransforms.Length){
            missesInRow++;
            selectionIndex += (change / Mathf.Abs(change));
            while(selectionIndex >= selectionTransforms.Length){selectionIndex -= selectionTransforms.Length;}
            while(selectionIndex < 0){selectionIndex += selectionTransforms.Length;}
        }

        selectorTransform.position = selectionTransforms[selectionIndex].position;
    }

    public void SelectOption(){

        string prompt = "";
        switch(selectionIndex){
            case 0: //new game
                prompt = "Start new game?";
            break;
            case 1: //load game
                prompt = "Load previous progress?";
            break;
            case 2: //settings
                optionsMenu.StartSelection();
                StopSelecting();
                return;
            case 3: //credits
                creditsObj.SetActive(true);
                creditsIgnoreInputFlag = true;
                return;
            case 4: //exit
                prompt = "Exit Game?";
            break;
            default:
            break;
        }
        waitingOnPrompt = true;
        popUp.PromptChoice(ChoiceType.Binary, FollowThroughSelection, prompt, "No", "Yes", true, 0.5f);
    }

    public void FollowThroughSelection(Result choiceResult){
        ignoreInputAfterPromptFlag = true;
        if(choiceResult.choiceResult){
            switch(selectionIndex){
                case 0: //new game
                    loader.StartLoadGame(true);
                break;
                case 1: //load game
                    loader.StartLoadGame(false);
                break;
                case 2: //settings
                break;
                case 3: //credits
                break;
                case 4: //exit
                    selectionUI.SetActive(false);
                    isSelecting = false;
                    Application.Quit();
                break;
                default:
                break;
            }
        }
        
    }

    public void StopSelecting(){
        isSelecting = false;
        startIndex = -1;
        selectionUI.SetActive(false);
    }

    public void StartSelection(){
        disabledSelections[1] = !(PlayerPrefs.HasKey("souls"));
        selectionIndex = 0;
        startIndex = 0;
        selectorTransform.position = selectionTransforms[selectionIndex].position;
        isSelecting = true;
        selectionUI.SetActive(true);
    }
}

