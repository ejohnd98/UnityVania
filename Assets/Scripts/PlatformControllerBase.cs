using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

abstract public class PlatformControllerBase : MonoBehaviour {

    protected RayHandler rayHandler;
    protected PhysicsInterpreter physicsState;
    protected AttackHandler attackHandler;
    protected Health hp;

    public GameObject mirrorPivot;
    public float damage = 5.0f;
    public bool ignoreHits = false;

    void Start() {
        rayHandler = GetComponent<RayHandler>();
        attackHandler = GetComponent<AttackHandler>();
        physicsState = GetComponent<PhysicsInterpreter>();
        hp = GetComponent<Health>();
    }
    public void SetSimActive(bool newState){
        rayHandler.simEnabled = newState;
        ignoreHits = !newState;
    }

    public void StopInputs(bool newState){
        rayHandler.ProvideInput(0.0f, false, false, false);
        rayHandler.FullHalt();
        rayHandler.stopInput = newState;
        ignoreHits = newState;
        
    }

    public bool SimActive(){
        return ignoreHits || rayHandler.SimActive();
    }

    public void ReceiveHit(GameObject other){
        
        PlatformControllerBase otherController = other.GetComponentInParent<PlatformControllerBase>();
        DamageObject damageObject = other.GetComponent<DamageObject>();
        if(!ignoreHits && otherController != null && !rayHandler.IsInvincible()){
            hp.DealDamage(otherController.damage);
            rayHandler.StartKnockback(other);
        }else if(!ignoreHits && damageObject != null && !rayHandler.IsInvincible()){
            hp.DealDamage(damageObject.damage);
            rayHandler.StartKnockback(other);
        }

    }

    public abstract void KillActor();

    public void FaceDirection(int dir){
        Vector3 locScale = mirrorPivot.transform.localScale;
        locScale.x = dir;
        mirrorPivot.transform.localScale = locScale;
    }
}
