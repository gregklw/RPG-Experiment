using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    //script for item slots
    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject; //returns gameobject or null
            }
            return null;
        }
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (ItemDragHandler.itemBeingDragged != null)
        {
            if (GetComponent<SlotState>().typeOfSlot == SlotState.TypeOfSlot.Inventory)
                ItemDragHandler.itemBeingDragged.GetComponent<ItemDragHandler>().droppedInSlot = true; //triggers whenever dropped in a slot

            if (ItemDragHandler.itemBeingDragged.GetComponent<Equipment>())
                if (ItemDragHandler.itemBeingDragged.GetComponent<Equipment>().ReturnEquipmentSlot().Equals(transform))
                    ItemDragHandler.itemBeingDragged.GetComponent<ItemDragHandler>().droppedInSlot = true;

            if (!item) //if item exists = true, otherwise null = false by default (no item in slot)
            {
                if (ItemDragHandler.itemBeingDragged.GetComponent<ItemDragHandler>().droppedInSlot)
                {
                    ItemDragHandler.itemBeingDragged.transform.SetParent(transform); //if this is empty slot and item is dragged over and released, item occupies this slot         
                    if (item.GetComponentInParent<SlotState>().typeOfSlot == SlotState.TypeOfSlot.Equipment)
                    {
                        item.GetComponent<Equipment>().Equip();
                    }
                    else GetComponent<SlotState>().ChangeToOccupiedState();
                }
            }
            else //if item does exists in slot
            {
                Transform oldItemSlot = ItemDragHandler.itemBeingDragged.GetComponent<ItemDragHandler>().startParent;
                if (ItemDragHandler.itemBeingDragged.GetComponent<ItemDragHandler>().droppedInSlot)
                {
                    //if the item in place is in the equipment panel and item being dragged is an equipment
                    if (item.GetComponentInParent<SlotState>().typeOfSlot == SlotState.TypeOfSlot.Equipment
                        && ItemDragHandler.itemBeingDragged.GetComponent<Equipment>())
                    {
                        if (ItemDragHandler.itemBeingDragged.GetComponent<Equipment>().CompareEquipmentType(item.GetComponent<Equipment>()))
                            ItemDragHandler.itemBeingDragged.GetComponent<Equipment>().SwapChange(oldItemSlot);
                    }
                    else
                    {
                        item.transform.SetParent(oldItemSlot);
                        ItemDragHandler.itemBeingDragged.transform.SetParent(transform);
                        oldItemSlot.GetComponent<SlotState>().ChangeToOccupiedState();
                    }
                }
            }
        }
    }
}
