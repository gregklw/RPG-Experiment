using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillNode_MeleeProficiency : PlayerSkill
{
    public override void SkillEffect()
    {
        PlayerStats.playerStats.attackcooldown -= 0.10f;
    }
}
