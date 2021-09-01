using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class PlayerController : MonoBehaviour {

    private RayHandler rayHandler;
    private AttackHandler attackHandler;

    void Start() {
        rayHandler = GetComponent<RayHandler>();
        attackHandler = GetComponent<AttackHandler>();
    }

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

        if(Input.GetKeyDown(KeyCode.X)){
            attackHandler.StartAttack();
        }
    }

    public void ReceiveHit(GameObject other){
        Debug.Log("Player got hit!");

        //deal damage

        rayHandler.StartKnockback(other);
    }
}
