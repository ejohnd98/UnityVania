using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerController : PlatformControllerBase {

    public InputHandler inputHandler;
    public static PlayerController instance;
    public Vector3 CenterOfPlayer {get { return rayHandler.center; }}

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }else{
            instance = this;
        }
    }

    void Update(){
        UpdateInput();
    }

    public void GrantDoubleJump(){
        rayHandler.maxAirJumps = Mathf.Max(rayHandler.maxAirJumps, 1);
    }

    public void GrantTripleJump(){
        rayHandler.maxAirJumps = Mathf.Max(rayHandler.maxAirJumps, 2);
    }

    public void GrantTallJump(){
        rayHandler.maxTallJumps = Mathf.Max(rayHandler.maxTallJumps, 1);
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
        SoundSystem.instance.PlayGameOver();
        FindObjectOfType<SaveSystem>().DeathPrompt();
        //Debug.Log("GAME OVER");
    }

    
}
