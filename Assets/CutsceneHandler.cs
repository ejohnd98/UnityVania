using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHandler : MonoBehaviour
{
    public DialogueBox dialogueBox;
    public AreaController areaController;
    public PlayerController player;

    public Cutscene currentCutscene;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.gameObject.SetActive(false);
    }

    private void Update() {
        if(currentCutscene != null && !currentCutscene.cutsceneActive){
            EndCutscene();
        }
    }

    public void BeginCutscene(Cutscene cutscene){
        currentCutscene = cutscene;

        //stop players and enemies
        areaController.SetEnemyState(false);
        player.StopInputs(true);
    }

    public void EndCutscene(){
        currentCutscene = null;
        areaController.SetEnemyState(true);
        player.StopInputs(false);
    }
}
