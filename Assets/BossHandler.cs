using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandler : MonoBehaviour
{
    public GameObject bossPrefab;
    public BossPhases currentPhase = BossPhases.NotStarted;

    public enum BossPhases{
        NotStarted,
        Phase1,
        Defeated,

        None
    };

    private void Start() {
        //ensure doors are open
    }

    private void Update() {
        switch (currentPhase){
            case BossPhases.NotStarted:
                break;
            
            case BossPhases.None:
            default:
                break;
        }
    }

    public void StartBoss(){
        currentPhase = BossPhases.Phase1;
    }
}
