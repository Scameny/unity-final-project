using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies

{
    public abstract class EffectStrategy
    {
        public abstract void StartEffect(GameObject user, IEnumerable<GameObject> targets);
    }
}
