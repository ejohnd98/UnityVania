using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    CutsceneHandler handler;

    public List<GameObject> cutsceneObjects;
    int cutsceneIndex;
    public bool cutsceneActive = false;
    bool waitingForDialogue = false;
    bool onlyOnce = true;
    //have list of gameobjects to enable
    //move from one to next when they are deactivited (timed actions)
    //once over, tell handler the cutscene is over

    // Start is called before the first frame update
    void Awake(){
        handler = FindObjectOfType<CutsceneHandler>();
        cutsceneObjects = new List<GameObject>();
        for (int i=0; i<transform.childCount; i++) {
            cutsceneObjects.Add(transform.GetChild(i).gameObject);
        }
    }

    //should really be in the handler instead of here
    private void Update() {
        if(cutsceneActive){
            if(!cutsceneObjects[cutsceneIndex].activeSelf || (waitingForDialogue && !handler.dialogueBox.IsReading())){
                cutsceneIndex++;
                waitingForDialogue = false;
                if(cutsceneIndex < cutsceneObjects.Count){
                    ActivateNextObject();
                }else{
                    handler.dialogueBox.gameObject.SetActive(false);
                    cutsceneActive = false;
                    if(onlyOnce){
                        this.gameObject.SetActive(false);
                    }
                }
                
            }
        }
    }

    public void StartCutscene(){
        cutsceneIndex = 0;
        handler.BeginCutscene(this);
        cutsceneActive = true;
        ActivateNextObject();
    }

    public void ActivateNextObject(){
        cutsceneObjects[cutsceneIndex].SetActive(true);
        DialogueEvent dialogue = cutsceneObjects[cutsceneIndex].GetComponent<DialogueEvent>();

        if(dialogue != null){
            handler.dialogueBox.gameObject.SetActive(true);
            waitingForDialogue = true;
            handler.dialogueBox.ChangePortrait(dialogue.portrait);
            handler.dialogueBox.GiveLine(dialogue.dialogueText);
        }
    }
}
