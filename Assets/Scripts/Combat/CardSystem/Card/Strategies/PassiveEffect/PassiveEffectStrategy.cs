using Abilities.Passive;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    [System.Serializable]
    public abstract class PassiveEffectStrategy
    {

        [SerializeField] PassiveSignal passiveSignal;
        public abstract void Evaluate(PassiveSignal signal, GameObject user, IEnumerable<GameObject> targets);

        public PassiveSignal GetPassiveSignal()
        {
            return passiveSignal;
        }
    }

}
