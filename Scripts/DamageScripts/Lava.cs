using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private float damageEveryXSeconds = 1.0f;
    private int addAmountToHealth = -5;
    private bool entered;

    private float DamagePlayer(float dmg, float currentHealth)
    {
        Debug.Log("Damaged");
        return currentHealth -= dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            entered = true;
            StartCoroutine("LavaDamage");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            entered = false;
            StopCoroutine("LavaDamage");
        }
    }

    IEnumerator LavaDamage()
    {
        while (entered)
        {
            PlayerHealthEvent.playerHealthEvent.AddHealth(addAmountToHealth);
            yield return new WaitForSeconds(damageEveryXSeconds);
        }
    }
}
