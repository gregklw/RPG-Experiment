using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller playerController;

    [SerializeField]
    GameObject inventoryObj;
    public float speedFactor;

    private void Awake()
    {
        playerController = this;
    }

    private void FixedUpdate()
    {
        Movements();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            PlayerFunctions.playerFunctions.PlayerAttack();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayerFunctions.playerFunctions.SpecialAbility();
        }
    }

    void Movements()
    {
        if (Input.GetKey(ControlInitializer.controlScript.controlDictionary["forwardKey"].thisKeyCode) &&
            Input.GetKey(ControlInitializer.controlScript.controlDictionary["leftKey"].thisKeyCode))
        {
            PlayerFunctions.playerFunctions.MoveForwardsLeft();
        }
        else if (Input.GetKey(ControlInitializer.controlScript.controlDictionary["leftKey"].thisKeyCode) &&
            Input.GetKey(ControlInitializer.controlScript.controlDictionary["backwardKey"].thisKeyCode))
        {
            PlayerFunctions.playerFunctions.MoveBackwardsLeft();
        }
        else if (Input.GetKey(ControlInitializer.controlScript.controlDictionary["backwardKey"].thisKeyCode) &&
            Input.GetKey(ControlInitializer.controlScript.controlDictionary["rightKey"].thisKeyCode))
        {
            PlayerFunctions.playerFunctions.MoveBackwardsRight();
        }
        else if (Input.GetKey(ControlInitializer.controlScript.controlDictionary["rightKey"].thisKeyCode) &&
            Input.GetKey(ControlInitializer.controlScript.controlDictionary["forwardKey"].thisKeyCode))
        {
            PlayerFunctions.playerFunctions.MoveForwardsRight();
        }
        else if (Input.GetKey(ControlInitializer.controlScript.controlDictionary["forwardKey"].thisKeyCode)) //call from the controlscript singleton
        {
            PlayerFunctions.playerFunctions.MoveForwards();
        }
        else if (Input.GetKey(ControlInitializer.controlScript.controlDictionary["leftKey"].thisKeyCode))
        {
            PlayerFunctions.playerFunctions.MoveLeft();
        }
        else if (Input.GetKey(ControlInitializer.controlScript.controlDictionary["backwardKey"].thisKeyCode))
        {
            PlayerFunctions.playerFunctions.MoveBackwards();
        }
        else if (Input.GetKey(ControlInitializer.controlScript.controlDictionary["rightKey"].thisKeyCode))
        {
            PlayerFunctions.playerFunctions.MoveRight();
        }
    }
}
