using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    Gamepad gamepad;
    Keyboard keyboard;

    public float h_axis;
    public float v_axis;
    public int jump_button;
    public int crouch_button;
    public int attack_button;

    public bool h_axis_held;
    public bool v_axis_held;
    public bool jump_button_held;
    public bool crouch_button_held;
    public bool attack_button_held;

    public bool h_axis_pressed;
    public bool v_axis_pressed;
    public bool jump_button_pressed;
    public bool crouch_button_pressed;
    public bool attack_button_pressed;

    public bool h_axis_released;
    public bool v_axis_released;
    public bool jump_button_released;
    public bool crouch_button_released;
    public bool attack_button_released;

    // Update is called once per frame
    void Update(){
        UpdateDevices();
        UpdateInputs();
    }

    void UpdateDevices(){
        gamepad = Gamepad.current;
        keyboard = Keyboard.current;
    }

    void UpdateInputs(){
        h_axis = 0;
        v_axis = 0;
        jump_button = 0;
        attack_button = 0;
        crouch_button = 0;
        
        if(gamepad != null){
            h_axis += gamepad.leftStick.x.ReadValue();
            v_axis += gamepad.leftStick.y.ReadValue();

            h_axis += gamepad.dpad.x.ReadValue();
            v_axis += gamepad.dpad.y.ReadValue();

            jump_button += gamepad.buttonSouth.isPressed? 1 : 0;
            attack_button += gamepad.buttonWest.isPressed? 1 : 0;
            crouch_button += (v_axis <= -0.15f)? 1 : 0;
        }
        if(keyboard != null){
            h_axis += (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)? 1 : 0;
            h_axis -= (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)? 1 : 0;

            v_axis += (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)? 1 : 0;
            v_axis -= (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)? 1 : 0;

            jump_button += keyboard.zKey.isPressed? 1 : 0;
            attack_button += keyboard.xKey.isPressed? 1 : 0;
            crouch_button += (v_axis <= -0.15f)? 1 : 0;
        }

        h_axis = Mathf.Clamp(h_axis, -1.0f, 1.0f);
        v_axis = Mathf.Clamp(v_axis, -1.0f, 1.0f);
        jump_button = Mathf.Clamp(jump_button, 0, 1);
        attack_button = Mathf.Clamp(attack_button, 0, 1);
        crouch_button = Mathf.Clamp(crouch_button, 0, 1);


        //h_axis = Input.GetAxisRaw("Horizontal");
        if(h_axis != 0 && !h_axis_held){
            h_axis_held = true;
            h_axis_pressed = true;
        }else if(h_axis != 0){
            h_axis_pressed = false;
        }else if(h_axis_held && h_axis == 0){
            h_axis_held = false;
            h_axis_released = true;
        }else if(h_axis_released){
            h_axis_released = false;
        }

        //v_axis = Input.GetAxisRaw("Vertical");
        if(v_axis != 0 && !v_axis_held){
            v_axis_held = true;
            v_axis_pressed = true;
        }else if(v_axis != 0){
            v_axis_pressed = false;
        }else if(v_axis_held && v_axis == 0){
            v_axis_held = false;
            v_axis_released = true;
        }else if(v_axis_released){
            v_axis_released = false;
        }

        //jump_button = (int)Input.GetAxisRaw("Jump");
        if(jump_button != 0 && !jump_button_held){
            jump_button_held = true;
            jump_button_pressed = true;
        }else if(jump_button != 0){
            jump_button_pressed = false;
        }else if(jump_button_held && jump_button == 0){
            jump_button_held = false;
            jump_button_released = true;
        }else if(jump_button_released){
            jump_button_released = false;
        }

        
        if(crouch_button != 0 && !crouch_button_held){
            crouch_button_held = true;
            crouch_button_pressed = true;
        }else if(crouch_button != 0){
            crouch_button_pressed = false;
        }else if(crouch_button_held && crouch_button == 0){
            crouch_button_held = false;
            crouch_button_released = true;
        }else if(crouch_button_released){
            crouch_button_released = false;
        }

        //attack_button = (int)Input.GetAxisRaw("Attack");
        if(attack_button != 0 && !attack_button_held){
            attack_button_held = true;
            attack_button_pressed = true;
        }else if(attack_button != 0){
            attack_button_pressed = false;
        }else if(attack_button_held && attack_button == 0){
            attack_button_held = false;
            attack_button_released = true;
        }else if(attack_button_released){
            attack_button_released = false;
        }
    }

    public bool CrouchHeld(){
        return v_axis <= -0.15f && v_axis_held;
    }
}
