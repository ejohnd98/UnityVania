using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    // use underscore _ as linebreak;
    Text textObj;
    public SpriteRenderer portrait;
    
    float textProgress = 0.0f;
    public float textSpeed = 1.0f;
    float defaultCharTime = 0.075f;
    string dialogueToRead = "";
    public string currentContents = "";
    bool readingText = false;
    int index = 0;

    bool delayNext = false;
    // Start is called before the first frame update
    void Start(){
        textObj = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update(){
        if(readingText && index < dialogueToRead.Length){
            textProgress += Time.deltaTime * textSpeed;
            if(textProgress >= (delayNext? defaultCharTime*2.0f : defaultCharTime)){
                delayNext = false;
                textProgress = 0.0f;
                RevealCharacter();
                textObj.text = currentContents;
            }
        }
    }

    void RevealCharacter(){
        char newChar = dialogueToRead[index];
        if(newChar == '.'){
            delayNext = true;
        }
        if (newChar == '_'){
            newChar = '\n';
        }
        currentContents+= newChar;
        index++;
        if(index >= dialogueToRead.Length){
            readingText = false;
        }
    }

    public bool IsReading(){
        return index < dialogueToRead.Length;
    }

    public void GiveLine(string newLine){
        dialogueToRead = newLine;
        textProgress = 0.0f;
        index = 0;
        currentContents = "";
        readingText = true;
    }

    public void ChangePortrait(Sprite newSprite){
        portrait.sprite = newSprite;
    }
}
