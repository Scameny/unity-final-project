using Abilities.Passive;
using Character.Character;
using Character.Stats;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    [System.Serializable]
    public class ResourceRecoverPassiveEffect : PassiveEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public int quantity;
        [LabelWidth(120)]
        [SerializeField] public ResourceType resourceType;

        override public void Evaluate(PassiveSignal signal, GameObject user, IEnumerable<GameObject> targets)
        {
            if (signal.Equals(GetPassiveSignal()))
                EffectActivation(user, targets);
        }

        void EffectActivation(GameObject user, IEnumerable<GameObject> targets)
        {
            user.GetComponent<DefaultCharacter>().GainResource(quantity, resourceType);
        }
    }

}
