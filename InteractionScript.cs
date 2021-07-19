using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionScript : MonoBehaviour
{
    [SerializeField]
    Transform optionsNPCBox;

    public static InteractionScript PlayerInteraction { get; private set; }
    private LinkedList<GameObject> interactableLL = new LinkedList<GameObject>();
    public static GameObject HighlightedObj { get; private set; }

    public bool InteractingWithNPC { get; set; }

    public bool InteractingWithItem { get; set; }

    private void Awake()
    {
        if (PlayerInteraction != null && PlayerInteraction != this)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            PlayerInteraction = this;
        }
        Physics2D.callbacksOnDisable = false;
    }

    private void Update()
    {
        if (HighlightedObj != null)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log(HighlightedObj);
                Interact();
            }
        }
    }

    private void Interact()
    {
        if (HighlightedObj != null && !MenuScript.menuScript.transform.GetChild(0).gameObject.activeSelf)
        {
            if (HighlightedObj.CompareTag("NPC"))
            {
                NPCInteractionHelper();
            }
            else if (HighlightedObj.CompareTag("Pickupable"))
            {
                if (HighlightedObj.name.Contains("Gold"))
                {
                    GoldInteractionHelper();
                }
                else
                {
                    ItemInteractionHelper();
                }
            }
            else if (HighlightedObj.CompareTag("Entrance"))
            {
                EntranceInteractionHelper();
            }
            else if (HighlightedObj.CompareTag("SceneExit"))
            {
                Debug.Log("asdas");
                SceneExitHelper();
            }
        }
    }

    private void NPCInteractionHelper()
    {
        InteractingWithNPC = true;
        if (ActivePlayerUI.activePlayerUI.linkedlist_of_all_ui.Count == 0)
        {
            ActivePlayerUI.activePlayerUI.Set_UI_Opened_Ref(optionsNPCBox.gameObject);
            ActivePlayerUI.activePlayerUI.linkedlist_of_all_ui.First.Value.SetActive(true);
            DisplayNPCOptions();
        }
    }

    private void GoldInteractionHelper()
    {
        int goldAmount = HighlightedObj.GetComponent<GoldAmount>().amount;
        PlayerGoldAmount.playerGoldAmount.ChangeGoldAmount(goldAmount);
        PopUpPromptScript.popUpPromptScript.pickupPopupFactory.CreatePickupPopupText(HighlightedObj.transform, Color.yellow, "+" + goldAmount + " Gold");
        HighlightedObj.SetActive(false);
    }

    private void ItemInteractionHelper()
    {
        Item objItemScript = HighlightedObj.GetComponent<Item>();
        EventLog.eventLog.SendMessageToLog(objItemScript.itemName + " has been picked up.", Color.black);
        PopUpPromptScript.popUpPromptScript.pickupPopupFactory.CreatePickupPopupText(HighlightedObj.transform, Color.blue, "+" + objItemScript.itemName);
        InventoryManager.inventoryManager.AddItemToAvailableSlot(HighlightedObj);
    }

    private void EntranceInteractionHelper()
    {
        if (HighlightedObj.GetComponent<Entrance>().locked)
        {
            if (HighlightedObj.GetComponent<Entrance>().HasKey())
            {
                HighlightedObj.GetComponent<Entrance>().locked = false;
                PopUpPromptScript.popUpPromptScript.regularPromptScript.ActivateRegularPrompt("Unlocked!");
            }
            else PopUpPromptScript.popUpPromptScript.regularPromptScript.ActivateRegularPrompt("This is locked.");
        }
        else HighlightedObj.GetComponent<Entrance>().MoveFunction(transform.parent);
    }

    private void SceneExitHelper()
    {
        SceneTracker.currentSceneName = HighlightedObj.name;
        HighlightedObj.GetComponent<ChangeLevel>().MoveToDestination();
    }

    private void DisplayNPCOptions()
    {
        foreach (MonoBehaviour mb in HighlightedObj.GetComponents<MonoBehaviour>())
        {
            if (mb is INPCFunctions)
            {
                (mb as INPCFunctions).GetNPCOptionButton().gameObject.SetActive(true);
                (mb as INPCFunctions).GetNPCOptionButton().onClick.AddListener((mb as INPCFunctions).SpecialFunction);
            }
        }
    }

    public void ClearButtonAction(Button button)
    {
        button.onClick.RemoveAllListeners();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC") || collision.CompareTag("Pickupable") || collision.CompareTag("Entrance") || collision.CompareTag("SceneExit")) //if player enters NPC vicinity
        {
            interactableLL.AddFirst(collision.gameObject);
            Color temp;

            if (HighlightedObj != null) //change old highlighted object
            {
                temp = HighlightedObj.GetComponent<SpriteRenderer>().color;
                temp.a = 1;
                HighlightedObj.GetComponent<SpriteRenderer>().color = temp;
            }
            HighlightedObj = interactableLL.First.Value;


            Debug.Log(interactableLL.Count);
            temp = HighlightedObj.GetComponent<SpriteRenderer>().color;
            temp.a = 0.5f;
            HighlightedObj.GetComponent<SpriteRenderer>().color = temp; //change it's colour to slightly transparent to indicate that it's the current one selected

            PopUpPromptScript.popUpPromptScript.interactionPopupHint.popupUtil.ShowPopupHint();
            PopUpPromptScript.popUpPromptScript.interactionPopupHint.popupUtil.RepositionPopup(HighlightedObj.transform);
            PopUpPromptScript.popUpPromptScript.interactionPopupHint.textComp.text = "Press 'R' to Interact with " + HighlightedObj.name;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC") || collision.CompareTag("Pickupable") || collision.CompareTag("Entrance") || collision.CompareTag("SceneExit")) //if player exits NPC vicinity
        {

            Debug.Log(HighlightedObj + "/" + interactableLL.First.Value);
            GameObject oldTopObj = null;
            if (InteractingWithItem)
                interactableLL.RemoveFirst();
            else
            {
                oldTopObj = interactableLL.Last.Value;
                interactableLL.RemoveLast();
            }

            Debug.Log(interactableLL.Count + "a");

            if (oldTopObj != null)
            {
                Color temp = HighlightedObj.GetComponent<SpriteRenderer>().color;
                temp.a = 1;
                HighlightedObj.GetComponent<SpriteRenderer>().color = temp;
            }

            if (interactableLL.Count > 0)
            {
                HighlightedObj = interactableLL.First.Value;
                Color temp = HighlightedObj.GetComponent<SpriteRenderer>().color;
                temp.a = 0.5f;
                HighlightedObj.GetComponent<SpriteRenderer>().color = temp;
                PopUpPromptScript.popUpPromptScript.interactionPopupHint.popupUtil.RepositionPopup(HighlightedObj.transform);
                PopUpPromptScript.popUpPromptScript.interactionPopupHint.textComp.text = "Press 'R' to Interact with " + HighlightedObj.name;
            }
            else //when exiting trigger of last NPC in vicinity
            {
                PopUpPromptScript.popUpPromptScript.interactionPopupHint.popupUtil.HidePopupHint();
                PopUpPromptScript.popUpPromptScript.interactionPopupHint.popupUtil.StopTransformationPopup();
                HighlightedObj = null;
            }
            InteractingWithItem = false;
        }
    }
}
