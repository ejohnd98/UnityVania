using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    
    private RayHandler rayHandler;
    private PhysicsInterpreter physicsState;
    public GameObject target;
    public float desiredDistanceH = 0.25f;
    public float desiredDistanceV = 1.5f;
    public float detectDistance = 20.0f;

    void Start() {
        rayHandler = GetComponent<RayHandler>();
        physicsState = GetComponent<PhysicsInterpreter>();
    }

    void Update(){
        UpdateInput();
    }

    void UpdateInput(){
        int xAxis = 0;
        bool jump = false;
        bool endJump = false;
        bool crouch = false;

        if (target != null && Vector2.Distance(transform.position, target.transform.position) < detectDistance){

            // Move towards target
            if (target.transform.position.x < transform.position.x - desiredDistanceH){
                xAxis = -1;
            }else if (target.transform.position.x > transform.position.x + desiredDistanceH){
                xAxis = 1;
            }

            // Jump if target is higher, or if running into wall
            if ((target.transform.position.y > transform.position.y + desiredDistanceV && rayHandler.velocity.y <= 0.0f)
                || (physicsState.CollideLeft() || physicsState.CollideRight())){
                jump = true;
            }else if(physicsState.LeavingGround() && target.transform.position.y >= transform.position.y - desiredDistanceV){
                rayHandler.airJumpsPerformed = -1;
                jump = true;
            }

            // Fall through platforms if target is below
            if (target.transform.position.y < transform.position.y - desiredDistanceV){
                crouch = true;
            }
        }

        rayHandler.ProvideInput(xAxis, jump, endJump, crouch);
    }

    public void ReceiveHit(GameObject other){
        Debug.Log("Enemy got hit!");

        //deal damage

        rayHandler.StartKnockback(other);
    }
}
