using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies

{
    public abstract class EffectStrategy : ScriptableObject
    {
        public abstract void StartEffect(GameObject user, IEnumerable<GameObject> targets);
    }
}
