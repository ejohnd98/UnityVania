using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 10.0f;
    public float currentHealth = 10.0f;

    public bool healthDepleted = false;

    public void DealDamage(float damageDone){
        currentHealth -= damageDone;
        if(currentHealth <= 0.0f){
            healthDepleted = true;
            currentHealth = Mathf.Clamp(currentHealth, 0.0f, maxHealth);

            //Kill unit
            PlatformControllerBase actor = GetComponent<PlatformControllerBase>();

            if(actor != null){
                actor.KillActor();
            }

        }else{
            currentHealth = Mathf.Clamp(currentHealth, 0.0f, maxHealth);
        }
    }
}
