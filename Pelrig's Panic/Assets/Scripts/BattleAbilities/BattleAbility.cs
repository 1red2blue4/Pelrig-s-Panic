using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAbility
{
    private string name;
    private string description;
    private Sprite icon;
    private List<AbilityBehaviors> behaviors;
    private bool requiresTarget;
    private bool canCastOnSelf;
    private int turnCooldown;
    //private GameObject particleEffect;        for another time. We will need to assign this when we create the ability

    public BattleAbility(string aname, Sprite aicon, List<AbilityBehaviors> abehaviors)
    {
        name = aname;
        icon = aicon;
        behaviors = new List<AbilityBehaviors>();
        behaviors = abehaviors;
        turnCooldown = 0;
        requiresTarget = false;
        canCastOnSelf = false;
        description = "Default";
    }

    public BattleAbility(string aname, Sprite aicon, List<AbilityBehaviors> abehaviors, bool thereIsATarget, int acooldown)
    {
        name = aname;
        icon = aicon;
        behaviors = new List<AbilityBehaviors>();
        behaviors = abehaviors;
        turnCooldown = acooldown;
        requiresTarget = thereIsATarget;
        canCastOnSelf = false;
        description = "Default";
    }

    public string getAbilityName
    {
        get { return name; }
    }

    public string getAbilityDescription
    {
        get { return description; }
    }

    public Sprite getAbilityIcon
    {
        get { return icon; }
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
