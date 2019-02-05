using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAbility
{
    private BasicObjectInformation objectInfo;
    private List<AbilityBehaviors> behaviors;
    private bool requiresTarget;
    private bool canCastOnSelf;
    private int turnCooldown;
    //private GameObject particleEffect;        for another time. We will need to assign this when we create the ability
    private AbilityType type;

    public enum AbilityType
    {
        SelfBuff,
        BuffTarget,
        DebuffEnemyTarget,
        DamageEnemyTarget,
        AreaOfEffect
    }

    public BattleAbility(BasicObjectInformation aBasicInfo, List<AbilityBehaviors> abehaviors)
    {
        objectInfo = aBasicInfo;
        behaviors = new List<AbilityBehaviors>();
        behaviors = abehaviors;
        turnCooldown = 0;
        requiresTarget = false;
        canCastOnSelf = false;
    }

    public BattleAbility(BasicObjectInformation aBasicInfo, List<AbilityBehaviors> abehaviors, bool thereIsATarget, int acooldown)
    {
        objectInfo = aBasicInfo;
        behaviors = new List<AbilityBehaviors>();
        behaviors = abehaviors;
        turnCooldown = acooldown;
        requiresTarget = thereIsATarget;
        canCastOnSelf = false;
    }

    public BasicObjectInformation AbilityInfo
    {
        get { return objectInfo; }
    }

    public int getAbilityTurnCooldown
    {
        get { return turnCooldown; }
    }

    public List<AbilityBehaviors> getAbilityBehaviors
    {
        get { return behaviors; }
    }

    public void useAbility()
    {

    }
}
