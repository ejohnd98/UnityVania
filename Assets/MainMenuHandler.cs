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

    public InputHandler inputHandler;

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
        if(isSelecting){
            if(inputHandler.v_axis_pressed){
                MoveSelector((int)Mathf.Sign(-inputHandler.v_axis));
            }
            if(inputHandler.attack_button_pressed){
                SelectOption();
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
        isSelecting = false;
        selectionUI.SetActive(false);
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
                Application.Quit();
            break;
            default:
            break;
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

