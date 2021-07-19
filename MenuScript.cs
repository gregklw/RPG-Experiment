using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    Transform menuPanel; //panel reference of the menu
    Event keyEvent; //reference for the key event we stored here
    Text buttonText; //text of the button
    KeyCode newKey; //new key used to overwrite 

    Transform controlsList; //transform containing the player controls

    bool waitingForKey; //boolean when waiting for player to enter key for keychange
    Transform waitingForKeyPrompt;

    public static MenuScript menuScript;

    private void Awake()
    {
        if (menuScript == null) //persist that only one GameManager exists throughout the whole game running duration
        {
            menuScript = this;
        }
        else if (menuScript != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        menuPanel = transform.Find("InGameMenu");
        controlsList = menuPanel.Find("ControlsList");
        menuPanel.gameObject.SetActive(false);
        waitingForKey = false;
        waitingForKeyPrompt = transform.Find("WFKPanel");
        waitingForKeyPrompt.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void ToggleMenu()
    {
        if (!menuPanel.gameObject.activeSelf) //escape to open and close menu        
        {
            if (ActivePlayerUI.activePlayerUI.linkedlist_of_all_ui.Count == 0)
            {
                ActivePlayerUI.activePlayerUI.Set_UI_Opened_Ref(gameObject);
                menuPanel.gameObject.SetActive(true);
                foreach (Transform child in menuPanel)
                {
                    if (child.name.Equals("InGameMenuList"))
                        child.gameObject.SetActive(true);
                    else
                        child.gameObject.SetActive(false);
                }
            }
        }
        else if (menuPanel.gameObject.activeSelf && !waitingForKeyPrompt.gameObject.activeSelf)
        {
            CloseUI();
        }
    }

    private void OnGUI()
    {
        keyEvent = Event.current; //keeps track of the current event which is called every frame (ex. pressing a key)

        if (keyEvent.isKey && waitingForKey)
        {
            if (keyEvent.keyCode.Equals(KeyCode.Escape))
                newKey = KeyCode.None;
            else if (keyEvent.keyCode.Equals(KeyCode.Delete))
                newKey = KeyCode.None;
            else
                newKey = keyEvent.keyCode; //next key pressed will be stored in newKey
            waitingForKey = false;
        }
    }

    public void OpenWidget(Transform targetWidget)
    {
        targetWidget.gameObject.SetActive(true);
    }

    public void HideWidget(Transform targetWidget)
    {
        targetWidget.gameObject.SetActive(false);
    }

    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }

    public void SendText(Text text)
    {
        buttonText = text;
    }

    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
        {
            waitingForKeyPrompt.gameObject.SetActive(true);
            yield return null;
        }
        waitingForKeyPrompt.gameObject.SetActive(false);
    }

    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;
        yield return WaitForKey();

        string clickedButtonText = buttonText.text; //text of the clicked button
        Control oldCtrl = ControlInitializer.controlScript.controlDictionary[keyName]; //reference to name of the Control obj to be edited
        KeyCode tempOldKeyCode = oldCtrl.thisKeyCode; //reference to keycode of the Control obj
        oldCtrl.thisKeyCode = KeyCode.None; //clear the keycode of the control obj corresponding to the clicked button
        string teststring = ""; //string reference for the json serialization

        foreach (KeyValuePair<string, Control> c in ControlInitializer.controlScript.controlDictionary)
        {
            if (newKey == c.Value.thisKeyCode)
            {
                Debug.Log("duplicate detected");
                c.Value.thisKeyCode = tempOldKeyCode;
                Text duplicateButtonText = controlsList.transform.Find(c.Value.keyName).GetComponentInChildren<Text>();
                Debug.Log(duplicateButtonText);
                duplicateButtonText.text = tempOldKeyCode.ToString();
                teststring = JsonHelper.ToJson(ControlInitializer.controlScript.controlArray, true); //save changes
                JsonHelper.SaveItemInfo(teststring, "testjson.json");
            }
        }

        ControlInitializer.controlScript.controlDictionary[keyName].thisKeyCode = newKey; //change the target control's keycode into the new keycode
        buttonText.text = newKey.ToString(); //change the clicked button's text display to the new key's string representation
        ControlInitializer.controlScript.UpdateControlListFromDictionary();
        teststring = JsonHelper.ToJson(ControlInitializer.controlScript.controlArray, true);
        JsonHelper.SaveItemInfo(teststring, "testjson.json");
    }

    public void CloseUI()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.SetParent(ActivePlayerUI.activePlayerUI.ui_canvas_group);
        ActivePlayerUI.activePlayerUI.linkedlist_of_all_ui.RemoveFirst();
        ActivePlayerUI.activePlayerUI.front_ui_parent.SetAsLastSibling();
        ActivePlayerUI.activePlayerUI.ui_in_focus = null;
    }

    public void ExitToTitle()
    {
        PopUpPromptScript.popUpPromptScript.confirmationPromptScript.ActivateConfirmationPrompt(
            delegate { SceneManager.LoadScene("TitleScreen"); }, "Return to Title? You will lose all current progress.");
    }
}
