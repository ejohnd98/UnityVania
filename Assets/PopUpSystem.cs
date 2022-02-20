using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public struct Result{
    public bool choiceResult;
    public float sliderResult;

    public Result(bool binary, float slider){
        choiceResult = binary;
        sliderResult = slider;
    }
}

public enum ChoiceType{
    Binary,
    Slider
};

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpObj, sliderLeft, sliderRight, sliderObj, sliderGroup, binaryGroup;
    public Text promptText, binaryFalseText, binaryTrueText, sliderLeftText, sliderRightText;
    public bool waitingOnInput = false;

    public InputHandler inputHandler;

    ChoiceType currentChoiceType;

    public Color selectColor, deselectColor;

    Action<Result> currentCallback;

    bool binaryChoice = false, startUpFlag = false;
    float sliderValue = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waitingOnInput){
            if(inputHandler.h_axis_pressed){
                MoveSelection((int)Mathf.Sign(inputHandler.h_axis));
            }
            if(inputHandler.attack_button_pressed){
                MakeChoice();
            }
            if(inputHandler.jump_button_pressed){
                switch(currentChoiceType){
                    case ChoiceType.Binary:
                        if(!binaryChoice){
                            MakeChoice();
                            break;
                        }
                        binaryChoice = false;
                        UpdateUI();
                    break;
                    case ChoiceType.Slider:
                        binaryChoice = false;
                        MakeChoice();
                    break;
                    default:
                    break;
                }
            }
        }
        if(startUpFlag){
            waitingOnInput = true;
            startUpFlag = false;
        }
    }

    void UpdateUI(){
        sliderObj.transform.position = Vector3.Lerp(sliderLeft.transform.position, sliderRight.transform.position, sliderValue);
        binaryFalseText.color = !binaryChoice? selectColor : deselectColor;
        binaryTrueText.color = binaryChoice? selectColor : deselectColor;
        
    }

    void MoveSelection(int direction){
        SoundSystem.instance.PlaySound("ui_move");
        switch(currentChoiceType){
            case ChoiceType.Binary:
                binaryChoice = !binaryChoice;
            break;
            case ChoiceType.Slider:
                sliderValue = Mathf.Clamp01(sliderValue + 0.1f * direction);
            break;
            default:
            break;
        }
        UpdateUI();
    }

    public void PromptChoice(ChoiceType choiceType, Action<Result> callback, string prompt, string falseText, string trueText, bool binaryDefault, float sliderDefault){
        promptText.text = prompt;
        currentChoiceType = choiceType;
        currentCallback = callback;
        switch(choiceType){
            case ChoiceType.Binary:
                binaryFalseText.text = falseText;
                binaryTrueText.text = trueText;
            break;
            case ChoiceType.Slider:
                sliderLeftText.text = falseText;
                sliderRightText.text = trueText;
            break;
            default:
            break;
        }
        binaryChoice = binaryDefault;
        sliderValue = sliderDefault;

        binaryGroup.gameObject.SetActive(choiceType == ChoiceType.Binary);
        sliderGroup.SetActive(choiceType == ChoiceType.Slider);

        UpdateUI();
        popUpObj.SetActive(true);
        startUpFlag = true;
        waitingOnInput = false;
        SoundSystem.instance.PlaySound("ui_move");
    }

    private void MakeChoice(){
        Debug.Log("choice made!");
        if(currentCallback != null){
            currentCallback(new Result(binaryChoice, sliderValue));
        }
        if(binaryChoice){
            SoundSystem.instance.PlaySound("ui_select", true);
        }else{
            SoundSystem.instance.PlaySound("ui_cancel", true);
        }
        popUpObj.SetActive(false);
        waitingOnInput = false;
        currentCallback = null;
    }
}
