using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossController : PlatformControllerBase {
    public GameObject target;
    public GameObject movingObject;
    public BossHandler handler;
    public Health[] healthSections;
    public GameObject[] healthSectionObjects;
    public int healthSectionsUsed = 0;

    public GameObject[] IntroObjects, Phase1Objects, Phase2Objects, Phase3Objects, DefeatedObjects;

    public enum BossAIState{
        Moving,
        Attacking,
        Defeated,

        None
    };

    public GameObject[] attackPrefabs, attackLocations;
    public UnityEvent[] attackEvents;
    public int numberOfAttacks = 0;

    //variables used by AI
    public BossAIState aiState = BossAIState.Moving;
    public Vector3 currentStart, currentDest;
    public BoxCollider2D bossArea;
    public float moveSpeed = 0.1f;
    public float moveProgress = 0.0f;
    public float currentStep = 1.0f;
    public bool attackingDone = false;
    public bool canMove = true;
    bool finishedMovingFlag = false;
    int attackNumber = 0; //used to alternate attacks

    private void Start() {
        healthSectionsUsed = healthSections.Length;
        numberOfAttacks = attackPrefabs.Length;
        currentStart = movingObject.transform.position;
        currentDest = movingObject.transform.position;
        bossArea = handler.bossArea;
    }

    void Update(){
        if(canMove){
            moveProgress += Time.deltaTime * currentStep;
            movingObject.transform.position = Vector3.Slerp(currentStart, currentDest, Easings.EaseInOutQuad(moveProgress));
            
            if(moveProgress >= 1.0f){
                finishedMovingFlag = true;
                currentStart = movingObject.transform.position;
                currentDest = new Vector3(Random.Range(bossArea.bounds.min.x, bossArea.bounds.max.x), Random.Range(bossArea.bounds.min.y, bossArea.bounds.max.y), movingObject.transform.position.z);
                moveProgress = 0.0f;
                currentStep = moveSpeed / ((currentStart - currentDest).magnitude);
            }
        }

        if(aiState == BossAIState.Moving){
            canMove = true;
            if(finishedMovingFlag){
                finishedMovingFlag = false;
                canMove = false;
                aiState = BossAIState.Attacking;
                if(handler.currentPhase == BossPhases.Phase1){
                    StartCoroutine(StartAttack(0));
                }
                if(handler.currentPhase == BossPhases.Phase2){
                    StartCoroutine(StartAttack(1));
                }
                if(handler.currentPhase == BossPhases.Phase3){
                    attackNumber++;
                    StartCoroutine(StartAttack(1 + attackNumber%2));
                }
                attackingDone = false;
            }
        }else if(aiState == BossAIState.Attacking){
            if(attackingDone){
                attackingDone = false;
                aiState = BossAIState.Moving;
            }
        }
    }

    IEnumerator StartAttack(int index){
        GameObject attackObj;
        switch (index){
            case 0:
                canMove = false;
                attackEvents[index].Invoke();
                attackObj = Instantiate(attackPrefabs[index], attackLocations[index].transform.position, attackLocations[index].transform.rotation, attackLocations[index].transform);
                yield return new WaitWhile(() => attackObj != null);
                attackingDone = true;
                break;
            case 1:
                attackObj = Instantiate(attackPrefabs[index], attackLocations[index].transform.position, attackLocations[index].transform.rotation, attackLocations[index].transform);
                attackEvents[index].Invoke();
                canMove = true;
                yield return new WaitWhile(() => attackObj != null);
                attackingDone = true;
                break;
            case 2:
                attackObj = Instantiate(attackPrefabs[index], attackLocations[index].transform.position, attackLocations[index].transform.rotation, attackLocations[index].transform);
                //attackEvents[index].Invoke();
                canMove = false;
                yield return new WaitWhile(() => attackObj != null);
                attackingDone = true;
                break;
            default:
                yield return null;
                break;
        }
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
