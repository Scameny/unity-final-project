using Strategies.TargetingStrategies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfUseSelection", menuName = "Strategy/TargetingSelection/SelfUseSelection", order = 2)]
public class SelfUseTargeting : TargetingStrategy
{
    public override void AbilityTargeting(GameObject user, IEnumerable<GameObject> targets, Action<IEnumerable<GameObject>> effectAction)
    {
        List<GameObject> toRet = new List<GameObject>();
        toRet.Add(user);
        effectAction.Invoke(toRet);
    }
}
