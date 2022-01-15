using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Door))]

public class ConditionalDoor : MonoBehaviour
{
    public ItemTypes requiredItem = ItemTypes.None;
    public bool proximityOpen = true, proximityClose = true;
    Door door;
    ItemHandler playerItems;

    // Start is called before the first frame update
    void Start()
    {
        door = GetComponent<Door>();
        playerItems = FindObjectOfType<ItemHandler>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(proximityOpen && playerItems.HasItem(requiredItem)){
            door.SetOpen(true, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(proximityClose){
            door.SetOpen(false, true);
        }
    }
}
