using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.TargetingStrategies
{ 
    public abstract class TargetingStrategy
    {
        public abstract void AbilityTargeting(GameObject user, IEnumerable<GameObject> targets, Action<IEnumerable<GameObject>, bool> effectAction);

    }
}