using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    public bool isSpecialAttack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHitbox enemyHitbox = collision.GetComponent<EnemyHitbox>();
            if (!isSpecialAttack)
            {
                PlayerProcs.procsOnHit?.Invoke(enemyHitbox);
                enemyHitbox.enemyHealthEvent.AddHealth(-(damage + PlayerProcs.procDamage));
            }
            else
            {
                enemyHitbox.enemyHealthEvent.AddHealth(-damage);
            }
            isSpecialAttack = false;
            PlayerProcs.procDamage = 0;
            if (!piercing)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
