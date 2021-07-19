using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGGameManager : MonoBehaviour
{
    public static RPGGameManager GM;

    /* declare all KeyCodes below */

    public KeyCode forwardKey { get; set; }
    public KeyCode leftKey { get; set; }
    public KeyCode backwardKey { get; set; }
    public KeyCode rightKey { get; set; }

    //public KeyCode esc { get; set; }

    private void Awake()
    {
        if (GM == null) //persist that only one GameManager exists throughout the whole game running duration
        {
            DontDestroyOnLoad(gameObject);
            GM = this;
        }
        else if (GM != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //forwardKey = ControlScript.controlScript.controlDictionary["forwardKey"].thisKeyCode;
        //leftKey = ControlScript.controlScript.GetControl("leftKey").thisKeyCode;
        //backwardKey = ControlScript.controlScript.GetControl("backwardKey").thisKeyCode;
        //rightKey = ControlScript.controlScript.GetControl("rightKey").thisKeyCode;
    }
}
