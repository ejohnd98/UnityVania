using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCreator : MonoBehaviour
{
    public void CreateAttack(GameObject prefab){
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
