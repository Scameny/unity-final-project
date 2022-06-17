using Abilities.Passive;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    public abstract class PassiveEffectStrategy
    {
        protected PassiveSignal signal;

        abstract public void Evaluate(PassiveSignal signal, GameObject user, IEnumerable<GameObject> targets);

        abstract protected void EffectActivation(GameObject user, IEnumerable<GameObject> targets);
    }

}
