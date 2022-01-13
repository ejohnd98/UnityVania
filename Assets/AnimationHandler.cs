using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator animator;
    public PhysicsInterpreter phys;
    public RayHandler rayHandler;
    public AttackHandler attack;

    void Update(){
        animator.SetBool("grounded", rayHandler.grounded);
        
        animator.SetFloat("hSpeed", rayHandler.velocity.x);
        animator.SetFloat("vSpeed", rayHandler.velocity.y);
        animator.SetBool("crouching", rayHandler.isCrouched);
        animator.SetBool("knockback", rayHandler.knockback);

        if(attack != null){
            animator.SetBool("attacking", attack.IsAttacking());
            if(attack.GetAttackFlag()){
                animator.SetTrigger("attackTrigger");
            }
        }
    }
}
