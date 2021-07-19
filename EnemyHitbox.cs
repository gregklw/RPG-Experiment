using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public EnemyHealthEvent enemyHealthEvent;
    public EnemyAI enemyAI;

    private void Start()
    {
        enemyHealthEvent = GetComponent<EnemyHealthEvent>();
    }
}
