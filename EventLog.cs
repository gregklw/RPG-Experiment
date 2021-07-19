using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Message
{
    public string text;
    public TextMeshProUGUI textObject;
}

public class EventLog : MonoBehaviour
{
    public static EventLog eventLog;
    private int maxMessages = 20;

    [SerializeField]
    GameObject chatPanel, textObject;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    private void Awake()
    {
        if (eventLog == null) //only one chatInterface exists at a time
        {
            eventLog = this;
        }
        else if (eventLog != this)
        {
            Destroy(gameObject);
        }
    }

    public void SendMessageToLog(string text, Color fontcolor)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = fontcolor;
        messageList.Add(newMessage);
    }
}
