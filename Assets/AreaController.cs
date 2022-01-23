using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Areas{
    Mansion = 0,
    Library,
    Cemetery,
    Tombs,
    Clocktower,
    Bridge,
    Preboss,
    Boss,

    None
};

public enum Enemies{
    Zombie = 0,

    None
}

public class AreaController : MonoBehaviour
{

    public Areas currentArea = Areas.None;
    public GameObject areaRoot;
    public GameObject playerObj;
    public SoundSystem sndSystem;

    public GameObject[] enemyPrefabs;
    public List<GameObject> currentEnemies;

    //debug
    public bool disableEnemies = false;

    // Start is called before the first frame update
    void Start()
    {
        currentEnemies = new List<GameObject>();
    }

    public void SetEnemyState(bool newState){
        foreach(GameObject obj in currentEnemies){
            obj.GetComponent<EnemyController>().SetSimActive(newState);
        }
    }

    public void UpdateArea (Areas newArea, Area obj){
        if(newArea == currentArea){
            return;
        }

        //despawn enemies
        foreach(GameObject en in currentEnemies){
            if(en != null){
                ObjectHandler.DestroyObjects(en.gameObject);
            }
        }

        sndSystem.ChangeMusic(newArea);

        currentArea = newArea;
        areaRoot = obj.gameObject;

        if(disableEnemies){
            return;
        }

        //spawn enemies
        EnemySpawn[] spawners = areaRoot.GetComponentsInChildren<EnemySpawn>();
        foreach(EnemySpawn spawn in spawners){
            GameObject enemy = GameObject.Instantiate(enemyPrefabs[(int)(spawn.enemyType)], spawn.transform.position, Quaternion.identity, areaRoot.transform);
            enemy.GetComponent<EnemyController>().target = playerObj;
            currentEnemies.Add(enemy);
        }

        //start boss
        BossHandler boss = areaRoot.GetComponentInChildren<BossHandler>();
        if(boss != null){
            boss.StartBoss();
        }

        //set background
        //toggle appropriate "fog of war" for other areas
    }
}
