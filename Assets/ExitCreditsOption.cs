using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCreditsOption : MonoBehaviour
{
    public SaveSystem saveSystem;
    public InputHandler inputHandler;
    public GameObject textObj;
    bool textVisible = false;

    // Update is called once per frame
    void Update()
    {
        if(textVisible){
            if(inputHandler.attack_button_held && inputHandler.jump_button_held){
                saveSystem.ReturnToMenu();
            }
        }else{
            if(inputHandler.attack_button_pressed || inputHandler.jump_button_pressed){
                textVisible = true;
                textObj.SetActive(true);
            }
        }
        
    }
}
