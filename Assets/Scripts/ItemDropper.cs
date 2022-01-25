using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public GameObject[] itemsToDrop;
    public GameObject soulPrefab;
    public int soulsToDrop = 2;
    public int overalSoulValue = 10;
    public float impulseStrength = 5.0f;

    public bool debugToggle = false;

    private void Update() {
        if (debugToggle){
            DropItems();
            debugToggle = false;
        }
    }

    public void DropItems(){
        foreach (GameObject i in itemsToDrop){
            SpawnItem(i);
        }
        for(int i = 0; i < soulsToDrop; i++){
            SpawnItem(soulPrefab, true);
        }
    }

    private void SpawnItem(GameObject prefab, bool giveValue = false){
        Vector3 pos = transform.position;
        pos.z = 0.1f;
        GameObject obj = GameObject.Instantiate(prefab, pos, Quaternion.identity);
        GameItem gameItem = obj.GetComponent<GameItem>();
        if (giveValue) { gameItem.value = overalSoulValue/soulsToDrop; }
        GiveImpulse(obj);
    }

    private void GiveImpulse(GameObject obj){
        Vector2 dir = new Vector2(Random.Range(-1.0f,1.0f), Random.Range(0.0f, 2.0f));
        obj.GetComponent<SimplePhysicsObject>().StartKnockback(dir.normalized, impulseStrength + Random.Range(-0.25f, 0.25f));
    }
}
