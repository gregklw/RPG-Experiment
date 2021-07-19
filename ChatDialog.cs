using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatDialog : MonoBehaviour, INPCFunctions
{
    public string[] dialog;
    private Queue<string> currentDialog = new Queue<string>();

    public bool IsChatting { get; set; }

    public void SpecialFunction()
    {   
        if (!UIWindowMasterScript.uiWindowMasterScript.dialogBoxUI.gameObject.activeSelf)
        {
            IsChatting = true;
            UIWindowMasterScript.uiWindowMasterScript.dialogBoxUI.gameObject.SetActive(true);
            UIWindowMasterScript.uiWindowMasterScript.dialogBoxUI.source = this;
            foreach (string s in dialog)
            {
                currentDialog.Enqueue(s);
            }
            UIWindowMasterScript.uiWindowMasterScript.dialogBoxUI.transform.GetComponentInChildren<TextMeshProUGUI>().text = currentDialog.Peek();
            currentDialog.Dequeue();
        }
        else
        {
            if (currentDialog.Count == 0)
            {
                ActivePlayerUI.activePlayerUI.CloseTargetUI(UIWindowMasterScript.uiWindowMasterScript.dialogBoxUI.gameObject);
            }
            else
            {
                UIWindowMasterScript.uiWindowMasterScript.dialogBoxUI.GetComponentInChildren<TextMeshProUGUI>().text = currentDialog.Peek();
                currentDialog.Dequeue();
            }
        }
    }

    public void CloseUI()
    {
        IsChatting = false;
        currentDialog = new Queue<string>();
        //OptionsNPCBox.optionsNPCBoxScript.chatButton.onClick.RemoveAllListeners();
        UIWindowMasterScript.uiWindowMasterScript.dialogBoxUI.gameObject.SetActive(false);
        UIWindowMasterScript.uiWindowMasterScript.dialogBoxUI.source = null;
    }

    public Button GetNPCOptionButton()
    {
        return OptionsNPCBox.optionsNPCBoxScript.chatButton;
    }

    public bool IsEmpty()
    {
        return dialog.Length == 0;
    }
}
