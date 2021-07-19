using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager inventoryManager;
    public List<Transform> inventorySlots = new List<Transform>();
    private readonly int numberOfSlots = 20;

    public InventoryPanel inventoryPanelScript;

    public GameObject inventoryPanelObj;

    [SerializeField]
    GameObject itemSlot;

    private void Awake()
    {
        if (inventoryManager == null) //singleton
        {
            inventoryManager = this;
        }
        else if (inventoryManager != this)
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < numberOfSlots; i++)
        {
            inventorySlots.Add(Instantiate(itemSlot, inventoryPanelScript.inventoryBackgroundT).transform);
        }

        foreach (Transform t in inventorySlots)
        {
            Color temp = t.GetComponent<Image>().color;
            temp.a = 0.5f;
            t.GetComponent<Image>().color = temp;
        }
    }

    private void Start()
    {
        inventoryPanelObj = inventoryPanelScript.gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!inventoryPanelObj.activeSelf)
            {
                inventoryPanelObj.SetActive(true);
                ActivePlayerUI.activePlayerUI.Set_UI_Opened_Ref_No_Freeze(inventoryPanelObj);
            }
            else
            {
                ActivePlayerUI.activePlayerUI.CloseTargetUINoFreeze(inventoryPanelObj);
            }
        }
    }

    public void AddItemToAvailableSlot(GameObject itemInScene)
    {
        itemInScene.GetComponent<Image>().enabled = true;
        itemInScene.GetComponent<CanvasGroup>().enabled = true;
        itemInScene.GetComponent<CanvasGroup>().blocksRaycasts = true;
        itemInScene.GetComponent<Collider2D>().enabled = false;
        if (GetInventoryItemCount() >= inventorySlots.Count)
        {
            PopUpPromptScript.popUpPromptScript.regularPromptScript.ActivateRegularPrompt("Inventory is full.");
        }
        else
        {
            foreach (Transform slot in inventorySlots)
            {
                if (!slot.GetComponent<ItemDropHandler>().item)
                {
                    itemInScene.transform.SetParent(slot);
                    itemInScene.transform.position = slot.position;
                    slot.GetComponent<SlotState>().ChangeToOccupiedState();
                    PlayerQuestLog.playerQuestLog.UpdateAllGivingObjectives(itemInScene);
                    break;
                }
            }
        }
    }

    public void DropItemOnGround(GameObject itemInSlot)
    {
        EventLog.eventLog.SendMessageToLog(name + " has been dropped.", Color.black);
        itemInSlot.GetComponent<Image>().enabled = false;
        itemInSlot.GetComponent<CanvasGroup>().enabled = false;
        itemInSlot.GetComponent<Collider2D>().enabled = true;
        itemInSlot.GetComponent<SpriteRenderer>().enabled = true;
        int dropdistancefromplayer = -2;
        itemInSlot.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, dropdistancefromplayer, 0);
        itemInSlot.transform.SetParent(null);
    }

    public int GetInventoryItemCount()
    {
        int count = 0;
        foreach (Transform slot in inventorySlots) { if (slot.childCount > 0) count++; }
        return count;
    }

    public List<GameObject> ReturnListOfObjectsFound(GameObject targetObject)
    {
        List<GameObject> listOfObjects = new List<GameObject>();
        foreach (Transform slot in inventorySlots) //then check all item slots for that same item
        {
            if (slot.childCount > 0)
            {
                if (slot.GetChild(0).GetComponent<Item>().itemName.Equals(targetObject.GetComponentInChildren<Item>().itemName))
                {
                    listOfObjects.Add(slot.GetChild(0).gameObject);
                }
            }
        }
        return listOfObjects;
    }
}
