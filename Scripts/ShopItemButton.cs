using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemButton : DoubleClickScript, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject item;
    public GameObject itemDisplayCopy;
    [SerializeField]
    Transform shopitemslot;
    public int price;
    ItemPreviewPanel itemPreviewPanel;

    private void Start()
    {
        itemDisplayCopy = Instantiate(item, shopitemslot);
        itemDisplayCopy.transform.SetAsFirstSibling();
        Destroy(itemDisplayCopy.GetComponent<ItemDragHandler>());
        itemPreviewPanel = PopUpPromptScript.popUpPromptScript.itemPreviewPanel;
    }

    protected override void DoubleClickEffect()
    {
        BuyItem();
    }

    public void BuyItem()
    {
        PopUpPromptScript.popUpPromptScript.confirmationPromptScript.ActivateConfirmationPrompt(BuyItemHelper, "Confirm purchase?");
    }

    private void BuyItemHelper()
    {
        if (PlayerGoldAmount.playerGoldAmount.GoldAmount >= price)
        {
            GameObject itemToAdd = Instantiate(item);
            InventoryManager.inventoryManager.AddItemToAvailableSlot(itemToAdd);
            PlayerGoldAmount.playerGoldAmount.ChangeGoldAmount(-price);
            EventLog.eventLog.SendMessageToLog(item.GetComponent<Item>().itemName + " has been purchased.", Color.blue);
        }
        else PopUpPromptScript.popUpPromptScript.regularPromptScript.ActivateRegularPrompt("Not enough gold.");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemPreviewPanel.RevealPreviewPanel(itemDisplayCopy);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemPreviewPanel.ClearAndClose();
    }
}
