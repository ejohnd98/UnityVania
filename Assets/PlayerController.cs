using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerController : PlatformControllerBase {

    void Update(){
        UpdateInput();
    }

    void UpdateInput(){
        int xAxis = 0;
        bool jumpInput = Input.GetKeyDown(KeyCode.Z);
        bool jumpRelease = Input.GetKeyUp(KeyCode.Z);

        if(Input.GetKey(KeyCode.LeftArrow)){
            xAxis -= 1;
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            xAxis += 1;
        }

        if(Input.GetKeyDown(KeyCode.DownArrow)){
            //rayHandler.SetCharHeight(1.25f);
        }
        if(Input.GetKeyUp(KeyCode.DownArrow)){
            //rayHandler.SetCharHeight(2.5f);
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
