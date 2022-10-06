using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.TargetingStrategies
{
    public class EverythingTargeting : TargetingStrategy
    {
        public override void AbilityTargeting(GameObject user, IEnumerable<GameObject> targets, Action<IEnumerable<GameObject>, bool> effectAction)
        {
            effectAction.Invoke(targets, true);
        }
    }
}