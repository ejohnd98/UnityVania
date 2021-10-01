using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerController : PlatformControllerBase {

    void Update(){
        UpdateInput();
    }

    void UpdateInput(){
        int xAxis = 0;
        bool jumpInput = Input.GetKeyDown(KeyCode.UpArrow);
        bool jumpRelease = Input.GetKeyUp(KeyCode.UpArrow);

        if(Input.GetKey(KeyCode.LeftArrow)){
            xAxis -= 1;
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            xAxis += 1;
        }

        if(attackHandler.IsAttacking() && rayHandler.grounded){
            xAxis = 0;
            jumpInput = false;
            jumpRelease = false;
        }
        
        rayHandler.ProvideInput(xAxis, jumpInput, jumpRelease, Input.GetKey(KeyCode.DownArrow));

        if(Input.GetKeyDown(KeyCode.X) && rayHandler.SimActive()){
            attackHandler.StartAttack();
        }

        FaceDirection(physicsState.FaceDir());
    }

    public override void KillActor(){
        rayHandler.SetSimState(false);
        Debug.Log("GAME OVER");
    }

    
}
