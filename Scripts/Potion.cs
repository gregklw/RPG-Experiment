using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Consumable
{
    public int amountToHeal = 35;
    protected override void ConsumableEffect()
    {
        PlayerHealthEvent.playerHealthEvent.AddHealth(amountToHeal);
    }
}
