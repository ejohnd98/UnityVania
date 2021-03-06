using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsInterpreter : MonoBehaviour
{
    private RayHandler rayHandler;
    private float collideDist = 0.25f;
    private bool lastGroundState = false;
    private bool lastGroundState2 = false; //I'm sorry
    public bool groundSound = false;
    bool firstGroundSound = true;

    void Start() {
        rayHandler = GetComponent<RayHandler>();
    }

    public bool InputAllowed(){
        return !(rayHandler.stopInput);
    }

    public bool CollideLeft(){
        return (rayHandler.leftCollide && rayHandler.leftDist >= -collideDist);
    }

    public bool CollideRight(){
        return (rayHandler.rightCollide && rayHandler.rightDist <= collideDist);
    }

    public bool CollideTop(){
        return (rayHandler.topCollide && rayHandler.topDist <= collideDist);
    }

    public bool CollideBottom(){
        return (rayHandler.bottomCollide && rayHandler.bottomDist >= -collideDist);
    }

    public bool LeavingGround(){
        if (!rayHandler.grounded && lastGroundState){
            lastGroundState = rayHandler.grounded;
            return true;
        }
        lastGroundState = rayHandler.grounded;
        return false;
    }

    public bool IsSliding(){
        return rayHandler.IsSliding();
    }

    public int FaceDir(){
        if(rayHandler.lastMoveDir == 0){
            return 1;
        }else{
            return rayHandler.lastMoveDir;
        }
        
    }



    public void Update() {
        if(groundSound && !lastGroundState2 && rayHandler.grounded){
            if(firstGroundSound){
                firstGroundSound = false;
            }else{
                SoundSystem.instance.PlaySound("groundNoise");
            }
        }
        lastGroundState2 = rayHandler.grounded;
    }


}
