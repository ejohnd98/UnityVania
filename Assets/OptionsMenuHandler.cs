using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuHandler : MonoBehaviour
{

    public GameObject selectionUI;
    public Transform selectorTransform;
    public Transform[] selectionTransforms;
    public int startIndex = 0;
    public int selectionIndex;
    public bool isSelecting = false;
    bool[] disabledSelections = {false, false, false, false, false};
    public bool waitingOnPrompt = false;
    public bool ignoreInputAfterPromptFlag = false;

    public InputHandler inputHandler;
    public PopUpSystem popUp;
    public MainMenuHandler mainMenu;
    public Text settingsTextValues;

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
        if(isSelecting && !waitingOnPrompt){
            if(inputHandler.v_axis_pressed){
                MoveSelector((int)Mathf.Sign(-inputHandler.v_axis));
            }
            if(inputHandler.attack_button_pressed){
                SelectOption();
            }
            if(inputHandler.jump_button_pressed){
                if(selectionIndex == selectionTransforms.Length - 1){
                    StopSelecting();
                }else{
                    selectionIndex = selectionTransforms.Length - 1;
                }
            }
        }
        if(ignoreInputAfterPromptFlag){
            ignoreInputAfterPromptFlag = false;
            waitingOnPrompt = false;
        }
    }

    public void MoveSelector(int change){
        selectionIndex += change;
        SoundSystem.instance.PlaySound("ui_move");
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

    public void UpdateOptionValues(){
        settingsTextValues.text = "[" + SettingsObject.instance.globalVolume.ToString("0.0") + "]\n";
        settingsTextValues.text += "[" + SettingsObject.instance.musicVolume.ToString("0.0") + "]\n";
        settingsTextValues.text += "[" + SettingsObject.instance.sfxVolume.ToString("0.0") + "]\n";
        settingsTextValues.text += "[" + SettingsObject.instance.screenShake.ToString("0.0") + "]";
    }

    public void SelectOption(){

        string prompt = "";
        float defaultValue = 0.5f;
        switch(selectionIndex){
            case 0: //new game
                prompt = "Set overall volume:";
                defaultValue = SettingsObject.instance.globalVolume;
            break;
            case 1: //load game
                prompt = "Set music volume:";
                defaultValue = SettingsObject.instance.musicVolume;
            break;
            case 2: //settings
                prompt = "Set SFX volume:";
                defaultValue = SettingsObject.instance.sfxVolume;
            break;
            case 3: //credits
                prompt = "Set screen shake amount:";
                defaultValue = SettingsObject.instance.screenShake;
            break;
            case 4: //exit
                StopSelecting();
                mainMenu.StartSelection();
                return;
            default:
            break;
        }
        waitingOnPrompt = true;
        popUp.PromptChoice(ChoiceType.Slider, FollowThroughSelection, prompt, "0.0", "1.0", true, defaultValue);
    }

    public void FollowThroughSelection(Result choiceResult){
        ignoreInputAfterPromptFlag = true;
        if(selectionIndex <= 3 && !choiceResult.choiceResult){
            return;
        }
        switch(selectionIndex){
            case 0: //overall volume
                SettingsObject.instance.globalVolume = choiceResult.sliderResult;
            break;
            case 1: //music
                SettingsObject.instance.musicVolume = choiceResult.sliderResult;
            break;
            case 2: //sfx
                SettingsObject.instance.sfxVolume = choiceResult.sliderResult;
            break;
            case 3: //screen shake
                SettingsObject.instance.screenShake = choiceResult.sliderResult;
            break;
            case 4: //exit
                StopSelecting();
                mainMenu.StartSelection();
            break;
            default:
            break;
        }
        UpdateOptionValues();
        SettingsObject.instance.SaveSettings();
        
    }

    public void StopSelecting(){
        isSelecting = false;
        startIndex = -1;
        selectionUI.SetActive(false);
    }

    public void StartSelection(){
        selectionIndex = 0;
        startIndex = 0;
        selectorTransform.position = selectionTransforms[selectionIndex].position;
        isSelecting = true;
        UpdateOptionValues();
        selectionUI.SetActive(true);
    }


        
}