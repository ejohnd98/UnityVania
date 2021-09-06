using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

abstract public class PlatformControllerBase : MonoBehaviour {

    protected RayHandler rayHandler;
    protected PhysicsInterpreter physicsState;
    protected AttackHandler attackHandler;
    protected Health hp;

    public float damage = 5.0f;

    void Start() {
        rayHandler = GetComponent<RayHandler>();
        attackHandler = GetComponent<AttackHandler>();
        physicsState = GetComponent<PhysicsInterpreter>();
        hp = GetComponent<Health>();
    }

    public bool SimActive(){
        return rayHandler.SimActive();
    }

    public void ReceiveHit(GameObject other){
        
        PlatformControllerBase otherController = other.GetComponentInParent<PlatformControllerBase>();
        if(otherController != null && !rayHandler.IsInvincible()){
            hp.DealDamage(otherController.damage);
            rayHandler.StartKnockback(other);
            
            Debug.Log("Actor got hit!");
        }

        
    }

    public abstract void KillActor();
}
