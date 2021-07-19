using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : EnemyAttack
{

    bool inside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inside = true;
            StartCoroutine("CR_DamageOverTime");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inside = false;
            StopCoroutine("CR_DamageOverTime");
        }
    }

    IEnumerator CR_DamageOverTime()
    {
        while (inside)
        {
            PlayerHealthEvent.playerHealthEvent.AddHealth(-enemyStats.damage);
            yield return new WaitForSeconds(2.5f);
        }
    }
}
