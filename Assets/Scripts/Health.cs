using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 10.0f;
    public float currentHealth = 10.0f;
    public SpriteModifier spriteMod;
    
    public ShakePresets shakeType = ShakePresets.SmallEnemy;

    public bool healthDepleted = false;

    public void DealDamage(float damageDone){
        currentHealth -= damageDone;
        ScreenShake.instance.StartShake(shakeType);
        spriteMod.FlashWhite();

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
