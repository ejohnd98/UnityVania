using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes{
    Gold,
    Key,
    DoubleJump,
    TripleJump,
    TallJump,

    None
};

public class ItemHandler : MonoBehaviour
{
    public List<ItemTypes> inventory;
    public PlayerController player;

    private void Start() {
        inventory = new List<ItemTypes>();
    }

    public void AddItem(ItemTypes newItem){
        if(newItem == ItemTypes.None){
            return;
        }

        inventory.Add(newItem);
        switch(newItem){
            case ItemTypes.DoubleJump:
                player.GrantDoubleJump();
                break;
            case ItemTypes.TripleJump:
                player.GrantTripleJump();
                break;
            case ItemTypes.TallJump:
                player.GrantTallJump();
                break;
            default:
                break;
        }
    }

    public bool HasItem(ItemTypes itemType){
        return inventory.Contains(itemType);
    }

    private void OnTriggerEnter2D(Collider2D other){
        GameItem pickup = other.GetComponent<GameItem>();
        AddItem(pickup.itemType);
        Destroy(pickup.gameObject);
    }
}
