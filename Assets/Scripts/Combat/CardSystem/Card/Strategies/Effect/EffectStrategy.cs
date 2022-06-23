using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies

{
    [Serializable]
    public abstract class EffectStrategy
    {
        abstract public void StartEffect(GameObject user, IEnumerable<GameObject> targets);
    }
}
