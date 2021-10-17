using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerController : PlatformControllerBase {

    public InputHandler inputHandler;

    void Update(){
        UpdateInput();
    }

    void UpdateInput(){
        float xAxis = inputHandler.h_axis;
        bool jumpInput = inputHandler.jump_button_pressed;
        bool jumpRelease = inputHandler.jump_button_released;

        if(attackHandler.IsAttacking() && rayHandler.grounded){
            xAxis = 0;
            jumpInput = false;
            jumpRelease = false;
        }
        
        rayHandler.ProvideInput(xAxis, jumpInput, jumpRelease, inputHandler.CrouchHeld());

        if(inputHandler.attack_button_pressed && rayHandler.SimActive()){
            attackHandler.StartAttack();
        }

        FaceDirection(physicsState.FaceDir());
    }

    public override void KillActor(){
        rayHandler.SetSimState(false);
        Debug.Log("GAME OVER");
    }

    
}
