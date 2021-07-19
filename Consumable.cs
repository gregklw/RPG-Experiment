using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Consumable : DoubleClickScript
{
    protected override void DoubleClickEffect()
    {
        UseConsumable();
        PopUpPromptScript.popUpPromptScript.itemPreviewPanel.ClearAndClose();
    }

    private void UseConsumable()
    {
        ConsumableEffect();
        GetComponentInParent<SlotState>().ChangeToVacantState();
        transform.SetParent(null);
        gameObject.SetActive(false);
    }

    protected abstract void ConsumableEffect();
}
