using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes{
    Soul,
    Key,
    DoubleJump,
    TripleJump,
    TallJump,

    None
};

public class ItemHandler : MonoBehaviour
{
    public List<ItemTypes> inventory;
    public int souls = 0;
    public PlayerController player;

    private void Start() {
        inventory = new List<ItemTypes>();
    }

    public void AddItem(GameItem newItem){
        if(newItem.itemType == ItemTypes.None){
            return;
        }
        if(newItem.itemType == ItemTypes.Soul){
            souls += newItem.value;
        }else{
            inventory.Add(newItem.itemType);
            switch(newItem.itemType){
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
        
    }

    public bool HasItem(ItemTypes itemType){
        return inventory.Contains(itemType);
    }

    private void OnTriggerEnter2D(Collider2D other){
        GameItem pickup = other.GetComponent<GameItem>();
        AddItem(pickup);
        ObjectHandler.DestroyObjects(pickup.gameObject);
    }
}
