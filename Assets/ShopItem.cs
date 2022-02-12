using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public ItemTypes gameItemType = ItemTypes.None;
    public UnityEvent boughtAction; //use for upgrades
    public string itemName, description;
    public int cost = 1;
    public int quantity = 1;
    public Sprite itemSprite;

    public Text nameText, costText;
    public SpriteRenderer spriteRenderer;

}
