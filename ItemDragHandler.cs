using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static GameObject itemBeingDragged;
    Vector3 startPosition; //used to reset position of item incase it gets dropped in invalid location
    public Transform startParent; //used for cases when item is dropped into a different slot than original
    public bool droppedInSlot;
    private ItemPreviewPanel itemPreviewPanel;

    private void Start()
    {
        itemPreviewPanel = PopUpPromptScript.popUpPromptScript.itemPreviewPanel;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.parent.position;
        startParent = transform.parent;

        if (startParent.GetComponent<SlotState>().typeOfSlot == SlotState.TypeOfSlot.Equipment) //if start parent is in equipment
        {
            itemBeingDragged.GetComponent<Equipment>().UnEquip();
        }
        else startParent.GetComponent<SlotState>().ChangeToVacantState();
        GetComponent<CanvasGroup>().blocksRaycasts = false; //in order to pass event THROUGH the item we're dragging
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.SetParent(ItemParentWhileDragged.itemDragParent.transform);
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true; //disable it when drag ends

        //if dropped outside box, create a dropped version in scene
        bool insideInventory = RectTransformUtility.RectangleContainsScreenPoint(
            InventoryManager.inventoryManager.inventoryPanelScript.inventoryBackgroundT.GetComponent<RectTransform>(), Input.mousePosition)
            && InventoryManager.inventoryManager.inventoryPanelScript.gameObject.activeSelf;
        bool insideEquipment = RectTransformUtility.RectangleContainsScreenPoint(
            EquipmentManager.equipmentManager.equipmentPanelScript.equipmentBackgroundT.GetComponent<RectTransform>(), Input.mousePosition)
            && EquipmentManager.equipmentManager.equipmentPanelScript.gameObject.activeSelf;

        if (!insideInventory && !insideEquipment)
        {
            InventoryManager.inventoryManager.DropItemOnGround(gameObject);
        }
        //scenario if dropped within current item panel but no slot then return to original slot
        else if (!droppedInSlot)
        {
            transform.SetParent(startParent);
            transform.position = startPosition;
            if (startParent.GetComponent<SlotState>().typeOfSlot == SlotState.TypeOfSlot.Equipment)
            {
                itemBeingDragged.GetComponent<Equipment>().Equip();
            }
            else startParent.GetComponent<SlotState>().ChangeToOccupiedState();
        }

        droppedInSlot = false;
        itemBeingDragged = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!itemBeingDragged)
        {
            itemPreviewPanel.RevealPreviewPanel(gameObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemPreviewPanel.gameObject.SetActive(false);
        itemPreviewPanel.ClearAndClose();
    }
}
