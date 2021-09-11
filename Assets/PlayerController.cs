using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerController : PlatformControllerBase {

    void Update(){
        UpdateInput();
    }

    void UpdateInput(){
        int xAxis = 0;
        if(Input.GetKey(KeyCode.LeftArrow)){
            xAxis -= 1;
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            xAxis += 1;
        }
        rayHandler.ProvideInput(xAxis, Input.GetKeyDown(KeyCode.UpArrow), Input.GetKeyUp(KeyCode.UpArrow), Input.GetKey(KeyCode.DownArrow));

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
