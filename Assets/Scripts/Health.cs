using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 10.0f;
    public float currentHealth = 10.0f;
    public SpriteModifier spriteMod;
    
    public ShakePresets shakeType = ShakePresets.SmallEnemy;
    public bool hasSound = false;
    public string hurtSound, deathSound;
    public bool useGenericSound = true;

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
                if(hasSound){
                    SoundSystem.instance.PlaySound(deathSound);
                    if(useGenericSound){
                        SoundSystem.instance.PlaySound("genericHit");
                    }
                }
            }

        }else{
            if(hasSound){
                SoundSystem.instance.PlaySound(hurtSound);
                if(useGenericSound){
                    SoundSystem.instance.PlaySound("genericHit");
                }
            }
            currentHealth = Mathf.Clamp(currentHealth, 0.0f, maxHealth);
        }
    }
}
