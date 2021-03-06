using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text healthText;
    public Text soulsText;
    public Health playerHealth;
    public ItemHandler playerInventory;

    public Transform healthBar;
    public Transform healthBarFullPos;
    public Transform healthBarDepletedPos;

    public GameObject miniMapUI;

    public GameObject bossUI;
    bool bossUIActive = false;
    public Transform bossHealthBar;
    public Transform bossHealthBarFullPos;
    public Transform bossHealthBarDepletedPos;

    public BossHandler bossHandler;
    public PixelPerfectUIScaler uiScaler;

    int lastPlayerHealth = 0;

    // Update is called once per frame
    void Update(){
        healthText.text = playerHealth.currentHealth.ToString() + "/" + playerHealth.maxHealth.ToString();
        soulsText.text = playerInventory.souls.ToString();

        float healthFrac = (float)playerHealth.currentHealth / (float)playerHealth.maxHealth;
        healthBar.position = Vector3.Lerp(healthBarDepletedPos.position, healthBarFullPos.position, healthFrac);

        //messy way of checking for changes in player health
        int currentPlayerHealth = (int)playerHealth.currentHealth;
        if(currentPlayerHealth != lastPlayerHealth){
            FlashPlayerHealth();
            lastPlayerHealth = currentPlayerHealth;
        }
    }

    public void SetBossUI(BossHandler handler){
        bossHandler = handler;
        bossUI.SetActive(handler != null);
        bossUIActive = (handler != null);
        miniMapUI.SetActive(handler == null);
    }

    public void SetBossHealth(int current, int max){
        float healthFrac = (float)current / (float)max;
        bossHealthBar.position = uiScaler.ppc.RoundToPixel(Vector3.Lerp(bossHealthBarDepletedPos.position, bossHealthBarFullPos.position, healthFrac));
    }

    public void FlashBossHealth(){
        bossHealthBar.GetComponent<SpriteModifier>().CreateDeathSprite();
    }

    public void FlashPlayerHealth(){
        healthBar.GetComponent<SpriteModifier>().CreateDeathSprite();
    }
}
