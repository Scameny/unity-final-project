using Strategies.TargetingStrategies;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTargeting", menuName = "Strategy/TargetingSelection/EnemyTargeting", order = 2)]
public class EnemyTargeting : TargetingStrategy
{
    public override void AbilityTargeting(GameObject user, IEnumerable<GameObject> targets, Action<IEnumerable<GameObject>, bool> effectAction)
    {
        List<GameObject> toRet = new List<GameObject>();
        foreach (var target in targets)
        {
            toRet.Add(target);
            effectAction.Invoke(toRet,true);
        }
    }
}