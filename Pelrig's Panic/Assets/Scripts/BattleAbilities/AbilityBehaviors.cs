﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBehaviors
{
    private BasicObjectInformation objectInfo;
    private BehaviorStartTimes startTime;

    public AbilityBehaviors(BasicObjectInformation basicInfo, BehaviorStartTimes sTime)
    {
        objectInfo = basicInfo;
        startTime = sTime;
    }

    public enum BehaviorStartTimes
    {
        Beginning,
        Middle,
        End
    }

    public virtual void PerformBehavior()
    {

    }

    public BasicObjectInformation AbilityBehaviorInfo
    {
        get { return objectInfo; }
    }

    public BehaviorStartTimes AbilityBehaviorStartTime
    {
        get { return startTime; }
    }
}
