using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text healthText;
    public Health playerHealth;

    // Update is called once per frame
    void Update(){
        healthText.text = playerHealth.currentHealth.ToString() + "/" + playerHealth.maxHealth.ToString();
    }
}
