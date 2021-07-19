using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFunctions : MonoBehaviour
{
    public bool ToggleMovements { get; set; }
    public bool ToggleAttack { get; set; }

    public static PlayerFunctions playerFunctions;
    PlayerStats playerStats;

    public Weapon weapon;

    private void Awake()
    {
        playerFunctions = this;
        ToggleMovements = true;
    }

    private void Start()
    {
        playerStats = PlayerStats.playerStats;
    }

    public void PlayerAttack()
    {
        if (ToggleAttack && weapon)
        {
            StopCoroutine("AttackCooldown");
            StopCoroutine("StopMovementForDuration");
            weapon.DefaultAttack();
            StartCoroutine("StopMovementForDuration", 0.2f);
            StartCoroutine("AttackCooldown", playerStats.attackcooldown);
        }
    }

    public void SpecialAbility()
    {
        if (weapon is ISpecialWeaponAbility)
        {
            weapon.GetComponent<ISpecialWeaponAbility>().SpecialWeaponAbility();
        }
    }

    public void MoveForwards()
    {
        if (ToggleMovements)
        {
            transform.position += Vector3.up * playerStats.playerSpeed;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void MoveLeft()
    {
        if (ToggleMovements)
        {
            transform.position += Vector3.left * playerStats.playerSpeed;
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }

    public void MoveBackwards()
    {
        if (ToggleMovements)
        {
            transform.position += Vector3.down * playerStats.playerSpeed;
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
    }

    public void MoveRight()
    {
        if (ToggleMovements)
        {
            transform.position += Vector3.right * playerStats.playerSpeed;
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
    }

    public void MoveForwardsLeft()
    {
        if (ToggleMovements)
        {
            transform.position += (Vector3.up + Vector3.left) * playerStats.playerSpeed;
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void MoveBackwardsLeft()
    {
        if (ToggleMovements)
        {
            transform.position += (Vector3.left + Vector3.down) * playerStats.playerSpeed;
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }

    public void MoveBackwardsRight()
    {
        if (ToggleMovements)
        {
            transform.position += (Vector3.down + Vector3.right) * playerStats.playerSpeed;
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
    }

    public void MoveForwardsRight()
    {
        if (ToggleMovements)
        {
            transform.position += (Vector3.right + Vector3.up) * playerStats.playerSpeed;
            transform.eulerAngles = new Vector3(0, 0, 270);
        }
    }

    IEnumerator StopMovementForDuration(float timeInSeconds)
    {
        ToggleMovements = false;
        yield return new WaitForSeconds(timeInSeconds);
        ToggleMovements = true;
    }

    IEnumerator AttackCooldown(float timeInSeconds)
    {
        ToggleAttack = false;
        yield return new WaitForSeconds(timeInSeconds);
        ToggleAttack = true;
    }
}
