using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class SaveSystemLoader : MonoBehaviour
{
    public SaveSystem saveSystem;
    public string gameSceneName, mainMenuSceneName;
    public bool allowSavingAnywhere = false;

    private void Awake() {
        SaveSystemLoader[] objs = GameObject.FindObjectsOfType<SaveSystemLoader>();

        if (objs.Length > 1){
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveGame(){
        saveSystem = FindObjectOfType<SaveSystem>();

        string inventoryString = "";
        foreach(ItemTypes itm in saveSystem.itemHandler.inventory){
            if(!inventoryString.Equals("")){
                inventoryString += ',';
            }
            inventoryString += ((int)itm).ToString();
        }
        PlayerPrefs.SetString("inventoryString", inventoryString);

        PlayerPrefs.SetInt("libraryBossDefeated", saveSystem.libraryBoss.currentPhase == BossPhases.Defeated ? 1 : 0);
        PlayerPrefs.SetInt("tombsBossDefeated", saveSystem.tombsBoss.currentPhase == BossPhases.Defeated ? 1 : 0);
        PlayerPrefs.SetInt("clockBossDefeated", saveSystem.clockBoss.currentPhase == BossPhases.Defeated ? 1 : 0);
        PlayerPrefs.SetInt("finalBossDefeated", saveSystem.finalBoss.currentPhase == BossPhases.Defeated ? 1 : 0);

        PlayerPrefs.SetInt("mansionPortal", saveSystem.mansionPortal.discovered ? 1 : 0);
        PlayerPrefs.SetInt("libraryPortal", saveSystem.libraryPortal.discovered ? 1 : 0);
        PlayerPrefs.SetInt("tombsPortal", saveSystem.tombsPortal.discovered ? 1 : 0);
        PlayerPrefs.SetInt("clockPortal", saveSystem.clockPortal.discovered ? 1 : 0);
        PlayerPrefs.SetInt("keepPortal", saveSystem.keepPortal.discovered ? 1 : 0);

        PlayerPrefs.SetInt("souls", saveSystem.itemHandler.souls);

        PlayerPrefs.SetFloat("posX", saveSystem.player.transform.position.x);
        PlayerPrefs.SetFloat("posY", saveSystem.player.transform.position.y);

        PlayerPrefs.Save();
    }

    public void StartLoadGame(bool newGame = false){
        if(!newGame && !PlayerPrefs.HasKey("souls")){
            return;
        }
        StartCoroutine(SceneTransition(gameSceneName, ()=> FinishLoadGame(newGame)));
    }

    public void ReturnToMainMenu(){
        StartCoroutine(SceneTransition(mainMenuSceneName, null));
    }

    public void FinishLoadGame(bool newGame){
        saveSystem = FindObjectOfType<SaveSystem>();
        if(newGame){
            BlackFade.instance.FadeIn(3.0f);
            saveSystem.StartNewGame();
            Debug.Log("New Game started!");

        }else{ //otherwise load saved data
            BlackFade.instance.FadeIn();
            Debug.Log("Previous Game resumed!");
            string inventoryString = PlayerPrefs.GetString("inventoryString");
            string[] itemStrings = inventoryString.Split(',');
            foreach(string itm in itemStrings){
                int convertedItem;
                if(int.TryParse(itm, out convertedItem)){
                    saveSystem.itemHandler.AddItemByType((ItemTypes)convertedItem);
                    if(convertedItem == (int)ItemTypes.DoubleJump){ saveSystem.doubleJump.SetActive(false); }
                    if(convertedItem == (int)ItemTypes.TallJump){ saveSystem.tallJump.SetActive(false); }
                    if(convertedItem == (int)ItemTypes.Key){ saveSystem.skullKey.SetActive(false); }
                }
            }

            if(PlayerPrefs.GetInt("libraryBossDefeated") == 1){saveSystem.libraryBoss.SetDefeated();}
            if(PlayerPrefs.GetInt("tombsBossDefeated") == 1){saveSystem.tombsBoss.SetDefeated();}
            if(PlayerPrefs.GetInt("clockBossDefeated") == 1){saveSystem.clockBoss.SetDefeated();}
            if(PlayerPrefs.GetInt("finalBossDefeated") == 1){saveSystem.finalBoss.SetDefeated();}

            saveSystem.mansionPortal.SetState(PlayerPrefs.GetInt("mansionPortal") == 1);
            saveSystem.libraryPortal.SetState(PlayerPrefs.GetInt("libraryPortal") == 1);
            saveSystem.tombsPortal.SetState(PlayerPrefs.GetInt("tombsPortal") == 1);
            saveSystem.clockPortal.SetState(PlayerPrefs.GetInt("clockPortal") == 1);
            saveSystem.keepPortal.SetState(PlayerPrefs.GetInt("keepPortal") == 1);

            saveSystem.itemHandler.souls = PlayerPrefs.GetInt("souls");
            Vector3 pos = saveSystem.player.transform.position;
            pos.x = PlayerPrefs.GetFloat("posX");
            pos.y = PlayerPrefs.GetFloat("posY");
            saveSystem.player.transform.position = FindObjectOfType<PixelPerfectCamera>().RoundToPixel(pos);
            CameraFollow cam = FindObjectOfType<CameraFollow>();
            cam.transform.position = saveSystem.player.transform.position;
            FindObjectOfType<SoundSystem>()?.AllowMusic();
        }
    }

    IEnumerator SceneTransition(string newScene, Action callback){
        BlackFade.instance.FadeOut();
        FindObjectOfType<SoundSystem>().FadeOut(0.5f);
        yield return new WaitUntil(() => BlackFade.instance.DoneFading());
        SceneManager.LoadScene(newScene);
        yield return new WaitWhile(() => !SceneManager.GetActiveScene().name.Equals(newScene));
        BlackFade.instance.SetOpacity(1.0f);
        callback?.Invoke();
    }
}
