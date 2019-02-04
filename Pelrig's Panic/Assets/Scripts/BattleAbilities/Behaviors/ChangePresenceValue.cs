using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChangePresenceValue : AbilityBehaviors
{
    private const string name = "ChangePresenceValue";
    private const string description = "Changes Presence Value";
    private const BehaviorStartTimes startTime = BehaviorStartTimes.Beginning;
    // private const Sprite icon = Resources.Load();

    private int presenceChangeValue;

    public ChangePresenceValue(int presenceValue) : base(new BasicObjectInformation(name, description), startTime)
    {

    }

    public int presnceChangeValue
    {
        get { return presenceChangeValue; }
    }
}
