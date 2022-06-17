using Abilities.Passive;
using Character.Character;
using Character.Stats;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    [Serializable]

    public class ResourceRecoverPassiveEffect : PassiveEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public int quantity;
        [LabelWidth(120)]
        [SerializeField] public ResourceType resourceType;

        public override void Evaluate(PassiveSignal signal, GameObject user, IEnumerable<GameObject> targets)
        {
            if (signal.Equals(this.signal))
                EffectActivation(user, targets);
        }

        protected override void EffectActivation(GameObject user, IEnumerable<GameObject> targets)
        {
            user.GetComponent<DefaultCharacter>().GainResource(quantity);
        }
    }

}
