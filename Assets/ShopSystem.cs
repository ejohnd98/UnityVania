using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public ItemHandler itemHandler;
    public PlayerController player;

    public GameObject selectionUI;
    public GameObject shopItemPrefab;
    public bool isSelecting = false;
    float cooldown = 0.5f;
    bool coolingDown = false;
    public bool waitingOnPrompt = false;
    public bool ignoreInputAfterPromptFlag = false;

    public InputHandler inputHandler;
    public PopUpSystem popUp;

    //debug
    public bool moveUp, moveDown;
    public Transform shopListTransform;
    public Transform shopUIList;
    public List<ShopItem> shopItems;
    public int selectionIndex = 0;
    public int shopItemsToShow = 4;
    public Color selectColor, unselectColor;
    public Text descriptionText;
    

    private void Awake() {
        
        shopItems = new List<ShopItem>();
        for (int i=0; i<shopListTransform.childCount; i++) {
            shopItems.Add(shopListTransform.GetChild(i).GetComponent<ShopItem>());
        }
    }
    private void Update() {
        if(isSelecting && !waitingOnPrompt){
            if(inputHandler.v_axis_pressed){
                MoveSelector((int)Mathf.Sign(-inputHandler.v_axis));
            }
            if(inputHandler.attack_button_pressed){
                SelectOption();
            }
            if(inputHandler.jump_button_pressed){
                StopSelecting();
            }
        }
        if(ignoreInputAfterPromptFlag){
            ignoreInputAfterPromptFlag = false;
            waitingOnPrompt = false;
        }
    }

    public void MoveSelector(int change){
        selectionIndex += change;
        if(selectionIndex >= shopItems.Count){
            selectionIndex = 0;
        }
        if(selectionIndex < 0){
            selectionIndex = shopItems.Count - 1;
        }

        UpdateUI();
    }

    void UpdateUI(){
        for (int i=0; i<shopUIList.childCount; i++) {
            Destroy(shopUIList.GetChild(i).gameObject);
        }
        int startIndex = Mathf.Min(selectionIndex ,Mathf.Max(0, shopItems.Count - shopItemsToShow));
        for(int i = startIndex; i < shopItems.Count && i <= startIndex+shopItemsToShow-1; i++){
            GameObject obj = Instantiate(shopItemPrefab, Vector3.zero, Quaternion.identity, shopUIList);
            ShopItem shopItem = obj.GetComponent<ShopItem>();
            shopItem.costText.text = shopItems[i].cost.ToString();
            shopItem.nameText.text = shopItems[i].itemName;
            shopItem.spriteRenderer.sprite = shopItems[i].itemSprite;

            shopItem.costText.color = (i == selectionIndex && shopItems[i].cost <= itemHandler.souls)? selectColor : unselectColor;
            shopItem.nameText.color = (i == selectionIndex)? selectColor : unselectColor;

            if(i == selectionIndex){
                descriptionText.text = shopItems[i].description;
            }
        }
    }

    public void SelectOption(){
        ShopItem itm = shopItems[selectionIndex];
        if(itemHandler.souls >= itm.cost){
            string prompt = "Buy " + itm.itemName + " for " + itm.cost + "?";
            popUp.PromptChoice(ChoiceType.Binary, BuyItem, prompt, "No", "Yes", true, 0.5f);
            waitingOnPrompt = true;
        }else{
            //play sound
        }
    }

    public void BuyItem(Result choice){
        ShopItem itm = shopItems[selectionIndex];
        if(choice.choiceResult && itemHandler.souls >= itm.cost){
            itemHandler.souls -= itm.cost;
            itemHandler.AddItemByType(itm.gameItemType);
            itm.boughtAction.Invoke();
            itm.quantity--;
            if(itm.quantity <= 0){
                shopItems.RemoveAt(selectionIndex);
                Destroy(itm.gameObject);
            }

            UpdateUI();
        }
        ignoreInputAfterPromptFlag = true;
    }

    public void StopSelecting(){
        isSelecting = false;
        waitingOnPrompt = false;
        selectionUI.SetActive(false);
        coolingDown = true;
        player.StopInputs(false);
        StartCoroutine(ShopCooldown());
    }

    public void StartSelection(string saveLocationName){
        if(coolingDown || shopItems.Count == 0){
            return;
        }
        selectionIndex = 0;
        UpdateUI();

        player.StopInputs(true);
        isSelecting = true;
        selectionUI.SetActive(true);
    }

    IEnumerator ShopCooldown(){
        yield return new WaitForSeconds(cooldown);
        coolingDown = false;
    }
}
