using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes{
    Gold,
    Key,
    DoubleJump,

    None
};

public class ItemHandler : MonoBehaviour
{
    public List<ItemTypes> inventory;

    private void Start() {
        inventory = new List<ItemTypes>();
    }

    public void AddItem(ItemTypes newItem){
        if(newItem == ItemTypes.None){
            return;
        }

        inventory.Add(newItem);
    }

    private void OnTriggerEnter2D(Collider2D other){
        GameItem pickup = other.GetComponent<GameItem>();
        AddItem(pickup.itemType);
        Destroy(pickup.gameObject);
    }
}
