using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : DoubleClickScript
{
    protected Transform playerHitboxT;
    protected EquipmentManager equipmentManager;
    protected PlayerStatsPanel playerStatsPanel;
    protected ItemPreviewPanel itemPreviewPanel;

    protected virtual void Start()
    {
        playerHitboxT = GameObject.FindGameObjectWithTag("Player").transform;
        equipmentManager = EquipmentManager.equipmentManager;
        playerStatsPanel = PlayerStats.playerStats.playerStatsPanel;
        itemPreviewPanel = PopUpPromptScript.popUpPromptScript.itemPreviewPanel;
    }

    public abstract void Equip();
    public abstract void UnEquip();
    public abstract void SwapChange(Transform itemToSwap);
    public abstract bool CompareEquipmentType(Equipment equipment);
    public abstract string ReturnStatsDescription();
    public abstract Transform ReturnEquipmentSlot();
}
