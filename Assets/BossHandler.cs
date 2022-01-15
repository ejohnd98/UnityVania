using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandler : MonoBehaviour
{
    public GameObject bossPrefab;
    GameObject bossObj;
    public BossPhases currentPhase = BossPhases.NotStarted;

    public AudioClip bossMusic;
    public SoundSystem sndSystem;
    public GameObject playerObj;
    public Door[] doors;

    public enum BossPhases{
        NotStarted,
        //intro animation
        Phase1,
        //other phases
        //death animation
        Defeated,

        None
    };

    private void Start() {
        ChangeState(BossPhases.NotStarted);
    }

    private void Update() {
        CheckExitConditions();
    }

    public void StartBoss(){
        if(currentPhase == BossPhases.NotStarted){
            ChangeState(BossPhases.Phase1);
        }
    }

    public void ChangeState(BossPhases newState){
        switch(newState){
            case BossPhases.NotStarted:
                SetDoors(true, true);
                break;

            case BossPhases.Phase1:
                SetDoors(false);
                sndSystem.ChangeMusic(bossMusic);
                bossObj = GameObject.Instantiate(bossPrefab, transform.position, Quaternion.identity, transform);
                bossObj.GetComponent<EnemyController>().target = playerObj;
                break;

            case BossPhases.Defeated:
                sndSystem.StopMusic();
                SetDoors(true);
                break;
            
            case BossPhases.None:
            default:
                break;
        }

        currentPhase = newState;
    }

    public void CheckExitConditions(){
        switch(currentPhase){
            case BossPhases.NotStarted:
                break;

            case BossPhases.Phase1:
                if(bossObj == null || bossObj.GetComponent<Health>().currentHealth <= 0){
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
}
