using Strategies.TargetingStrategies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfUseTargeting : TargetingStrategy
{
    public override void AbilityTargeting(GameObject user, IEnumerable<GameObject> targets, Action<IEnumerable<GameObject>, bool> effectAction)
    {
        List<GameObject> toRet = new List<GameObject>();
        toRet.Add(user);
        effectAction.Invoke(toRet, true);
    }
}
