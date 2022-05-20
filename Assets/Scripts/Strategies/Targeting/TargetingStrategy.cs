using Character.Abilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.TargetingStrategies
{ 
    public abstract class TargetingStrategy : ScriptableObject
    {
        public abstract void AbilityTargeting(GameObject user, IEnumerable<GameObject> targets, Action<IEnumerable<GameObject>> effectAction);

    }
}