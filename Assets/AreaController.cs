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

    public GameObject[] enemyPrefabs;
    public List<GameObject> currentEnemies;

    // Start is called before the first frame update
    void Start()
    {
        currentEnemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateArea (Areas newArea, GameObject newAreaRoot){
        if(newArea == currentArea){
            return;
        }

        //despawn enemies
        foreach(GameObject obj in currentEnemies){
            Destroy(obj);
        }

        //begin fade out of music

        currentArea = newArea;
        areaRoot = newAreaRoot;

        //spawn enemies
        EnemySpawn[] spawners = areaRoot.GetComponentsInChildren<EnemySpawn>();
        foreach(EnemySpawn spawn in spawners){
            GameObject enemy = GameObject.Instantiate(enemyPrefabs[(int)(spawn.enemyType)], spawn.transform.position, Quaternion.identity, areaRoot.transform);
            enemy.GetComponent<EnemyController>().target = playerObj;
            currentEnemies.Add(enemy);
        }

        
        //queue new music
        //set background
        //toggle appropriate "fog of war" for other areas
    }
}
