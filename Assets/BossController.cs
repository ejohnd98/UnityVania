using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : PlatformControllerBase {
    public GameObject target;
    public BossHandler handler;
    public Health[] healthSections;
    public GameObject[] healthSectionObjects;
    public int healthSectionsUsed = 0;

    public GameObject[] IntroObjects, Phase1Objects, Phase2Objects, Phase3Objects, DefeatedObjects;

    private void Start() {
        healthSectionsUsed = healthSections.Length;
    }

    void Update(){
        UpdateInput();
    }

    void UpdateInput(){
        
    }

    //There is a definitely a better way to do this
    public void SetPhaseObjects(BossPhases phase){
        Debug.Log ("setting phase objects: " + phase);
        foreach (GameObject obj in IntroObjects){obj.SetActive(false);}
        foreach (GameObject obj in Phase1Objects){obj.SetActive(false);}
        foreach (GameObject obj in Phase2Objects){obj.SetActive(false);}
        foreach (GameObject obj in Phase3Objects){obj.SetActive(false);}
        foreach (GameObject obj in DefeatedObjects){obj.SetActive(false);}
        switch(phase){
            case BossPhases.NotStarted:
                foreach (GameObject obj in IntroObjects){ObjectHandler.SetObjectActive(obj, true);}
                break;
            case BossPhases.Phase1:
                foreach (GameObject obj in Phase1Objects){ObjectHandler.SetObjectActive(obj, true);}
                break;
            case BossPhases.Phase2:
                foreach (GameObject obj in Phase2Objects){ObjectHandler.SetObjectActive(obj, true);}
                break;
            case BossPhases.Phase3:
                foreach (GameObject obj in Phase3Objects){ObjectHandler.SetObjectActive(obj, true);}
                break;
            case BossPhases.Defeated:
                foreach (GameObject obj in DefeatedObjects){ObjectHandler.SetObjectActive(obj, true);}
                break;
            default:
            break;
        }
    }

    public override void KillActor(){
    }

    public bool SectionKilled (int section){
        if(section < healthSectionsUsed){
            return healthSections[section].healthDepleted;
        }else{
            return false;
        }
    }

    public new void ReceiveHit(GameObject other){
        Debug.Log ("test");
    }

    public void ReceiveBossHit(int section){
        PlatformControllerBase otherController = target.GetComponent<PlatformControllerBase>();
        if(otherController != null){
            healthSections[section].DealDamage(otherController.damage);
        }
        if(healthSections[section].healthDepleted){
            if(healthSectionObjects[section] != null){
                ObjectHandler.SetObjectActive(healthSectionObjects[section], false);
            }
                
                
        }
    }
}
