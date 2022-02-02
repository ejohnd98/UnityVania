using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    public ItemHandler itemHandler;
    public PlayerController player;
    public BossHandler libraryBoss, tombsBoss, clockBoss, finalBoss;
    public Portal mansionPortal, libraryPortal, tombsPortal, clockPortal, keepPortal;
    public GameObject doubleJump, skullKey, tallJump;

    public SaveSystemLoader loader;

    public GameObject selectionUI;
    public Transform selectorTransform;
    public Transform[] selectionTransforms;
    public Text menuText;
    public int startIndex = 0;
    public int selectionIndex;
    public bool isSelecting = false;
    float cooldown = 0.5f;
    bool coolingDown = false;
    bool deathPrompt = false;
    bool[] disabledSelections = {false, false, false, false};

    public InputHandler inputHandler;

    //debug
    public bool moveUp, moveDown;

    private void Awake() {
        coolingDown = true;
    }

    private void Start() {
        loader = FindObjectOfType<SaveSystemLoader>();
        StartCoroutine(SaveCooldown());
    }

    private void Update() {
        if(moveUp){
            moveUp = false;
            MoveSelector(-1);
        }
        if(moveDown){
            moveDown = false;
            MoveSelector(1);
        }
        if(isSelecting){
            if(inputHandler.v_axis_pressed){
                MoveSelector((int)Mathf.Sign(-inputHandler.v_axis));
            }
            if(inputHandler.attack_button_pressed){
                SelectOption();
            }
            if(inputHandler.jump_button_pressed){
                StopSelecting();
            }
        }
    }

    public void MoveSelector(int change){
        selectionIndex += change;
        while(selectionIndex >= selectionTransforms.Length){selectionIndex -= selectionTransforms.Length;}
        while(selectionIndex < 0){selectionIndex += selectionTransforms.Length;}
        
        int missesInRow = 0;
        while(disabledSelections[selectionIndex] && missesInRow <= selectionTransforms.Length){
            missesInRow++;
            selectionIndex += (change / Mathf.Abs(change));
            while(selectionIndex >= selectionTransforms.Length){selectionIndex -= selectionTransforms.Length;}
            while(selectionIndex < 0){selectionIndex += selectionTransforms.Length;}
        }

        selectorTransform.position = selectionTransforms[selectionIndex].position;
    }

    public void SelectOption(){
        isSelecting = false;
        selectionUI.SetActive(false);
        coolingDown = true;
        switch(selectionIndex){
            case 0:
                StopSelecting();
            break;
            case 1:
                loader.SaveGame();
                StopSelecting();
            break;
            case 2:
                loader.StartLoadGame(false);
            break;
            case 3:
                loader.ReturnToMainMenu();
                StopSelecting();
            break;
            default:
                StopSelecting();
            break;
        }
        
    }

    public void StopSelecting(){
        isSelecting = false;
        startIndex = -1;
        selectionUI.SetActive(false);
        coolingDown = true;
        player.StopInputs(false);
        StartCoroutine(SaveCooldown());
    }

    public void DeathPrompt(){
        menuText.text = "Game Over\n-----------\nLoad Game -\nMain Menu -";
        disabledSelections[0] = true;
        disabledSelections[1] = true;
        deathPrompt = true;
        StartSelection("death");
    }

    public void StartSelection(string saveLocationName){
        if(coolingDown){
            return;
        }
        
        disabledSelections[2] = !(PlayerPrefs.HasKey("souls"));
        selectionIndex = 0;
        startIndex = 0;
        if(deathPrompt){
            selectionIndex = 2;
            startIndex = 2;
            if(disabledSelections[2]){
                selectionIndex = 3;
                startIndex = 3;
            }
        }
        selectorTransform.position = selectionTransforms[selectionIndex].position;

        player.StopInputs(true);
        isSelecting = true;
        selectionUI.SetActive(true);
    }

    IEnumerator SaveCooldown(){
        yield return new WaitForSeconds(cooldown);
        coolingDown = false;
    }
}
