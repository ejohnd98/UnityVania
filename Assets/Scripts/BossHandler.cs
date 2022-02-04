using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BossPhases{
        NotStarted,
        Intro,
        Phase1,
        Phase2,
        Phase3,
        Defeated,

        None
    };
public class BossHandler : MonoBehaviour
{
    public GameObject bossPrefab, bossDeadPrefab;
    GameObject bossObj;
    BossController bossController;
    public BossPhases currentPhase = BossPhases.NotStarted;

    public AudioClip bossMusic;
    public SoundSystem sndSystem;
    public GameObject playerObj;
    public Door[] doors;
    public BoxCollider2D bossArea;
    public UIController uiController;

    public GameObject introObject;
    public UnityEvent introEvent;

    

    private void Start() {
        ChangeState(BossPhases.NotStarted);
    }

    private void Update() {
        CheckExitConditions();
    }

    public void StartBoss(){
        if(currentPhase == BossPhases.NotStarted){
            ChangeState(BossPhases.Intro);
        }
    }

    public void SetDefeated(){ //used by save system;
        if(introObject != null){
            introObject.SetActive(false);
        }
        currentPhase = BossPhases.Defeated;
    }

    public void ChangeState(BossPhases newState){
        switch(newState){
            case BossPhases.NotStarted:
                SetDoors(true, true);
                
                break;
            case BossPhases.Intro:
                introEvent.Invoke();
                SetDoors(false);
                break;

            case BossPhases.Phase1:
                
                uiController.SetBossUI(this);
                sndSystem.ChangeMusic(bossMusic);
                bossObj = GameObject.Instantiate(bossPrefab, transform.position, Quaternion.identity, transform);
                bossController = bossObj.GetComponent<BossController>();
                if(bossController == null){
                    bossController = bossObj.GetComponentInChildren<BossController>();
                }
                bossController.target = playerObj;
                bossController.handler = this;
                break;

            case BossPhases.Defeated:
                uiController.SetBossUI(null);
                if(bossDeadPrefab != null){
                    GameObject.Instantiate(bossDeadPrefab, bossController.transform.position, Quaternion.identity, transform);
                }
                bossObj.GetComponentInChildren<ItemDropper>().DropItems();
                ObjectHandler.DestroyObjects(bossObj);
                sndSystem.StopMusic();
                SetDoors(true);
                
                break;
            
            case BossPhases.None:
            default:
                break;
        }
        if(bossController != null)
            bossController.SetPhaseObjects(newState);
        currentPhase = newState;
    }

    public void CheckExitConditions(){
        switch(currentPhase){
            case BossPhases.Intro:
                if(introObject == null || !introObject.activeSelf){
                    ChangeState(BossPhases.Phase1);
                }
                break;

            case BossPhases.Phase1:
                if(bossController.SectionKilled(0) || bossController.SectionKilled(1)){
                    ChangeState(BossPhases.Phase2);
                }
                break;

            case BossPhases.Phase2:
                if(bossController.SectionKilled(0) && bossController.SectionKilled(1)){
                    ChangeState(BossPhases.Phase3);
                }
                break;

            case BossPhases.Phase3:
                if(bossObj == null || bossController.SectionKilled(bossController.healthSectionsUsed - 1)){
                    ChangeState(BossPhases.Defeated);
                }
                break;
            
            case BossPhases.None:
            default:
                break;
        }
    }

    void SetDoors(bool open, bool force = false){
        foreach(Door door in doors){
            door.SetOpen(open, force);
        }
    }

    public void UpdateHealthBar(){
        int current = 0;
        int max = 0;
        foreach(Health health in bossController.healthSections){
            current += (int)health.currentHealth;
            max += (int)health.maxHealth;
        }
        uiController.SetBossHealth(current, max);
        uiController.FlashBossHealth();
        
    }
}
