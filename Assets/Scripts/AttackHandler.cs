using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour {

    public float attackCooldown = 2.0f;
    public float attackActiveTime = 1.0f;

    public GameObject attackCollider;

    PhysicsInterpreter physState;
    double timer = 0.0f;
    bool attackActive = false;
    bool attackCoolingDown = false;
    bool attackAnimFlag = false;
    int direction = 1;

    public bool StartAttack(){
        if(attackCoolingDown){
            return false;
        }else{
            ActivateAttack();
            return true;
        }
    }

    private void Start() {
        physState = GetComponent<PhysicsInterpreter>();
    }

    private void Update() {
        if (attackCoolingDown){
            timer += Time.deltaTime;

            if(attackActive && timer >= attackActiveTime){
                DisableAttack();
            }
            if(timer >= attackCooldown){
                StopCooldown();
            }
        }
    }

    private void ActivateAttack(){
        if(!physState.InputAllowed()){
            return;
        }

        SoundSystem.instance.PlaySound("attackSound");

        Vector3 tempScale = attackCollider.transform.localScale;
        tempScale.x = Mathf.Abs(tempScale.x) * physState.FaceDir();
        attackCollider.transform.localScale = tempScale;
        attackActive = true;
        attackCoolingDown = true;
        timer = 0.0f;
        ObjectHandler.SetObjectActive(attackCollider, true);
        attackAnimFlag = true;
    }

    private void DisableAttack(){
        attackActive = false;
        ObjectHandler.SetObjectActive(attackCollider, false);
    }

    private void StopCooldown(){
        attackCoolingDown = false;
        timer = 0.0f;
    }

    public bool IsAttacking(){
        return attackCoolingDown;
    }

    public bool GetAttackFlag(){
        bool ret = attackAnimFlag;
        attackAnimFlag = false;
        return ret;
    }

}
