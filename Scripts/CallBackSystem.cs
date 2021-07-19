using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBackSystem : MonoBehaviour
{
    public delegate void OnMessageReceived();

    private void Start()
    {
        OnMessageReceived test = WriteMessage;
        test();
    }

    void WriteMessage()
    {
        //Debug.Log("Received");
    }
}
