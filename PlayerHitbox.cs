using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public PlayerHealthEvent playerHealthEvent;

    private void Start()
    {
        playerHealthEvent = GetComponent<PlayerHealthEvent>();
    }
}
