using Abilities.Passive;
using Character.Character;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    public class FlatDamagePassiveEffect : PassiveEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public int damage;
        [LabelWidth(120)]
        [SerializeField] public DamageType damageType;

        override public void Evaluate(PassiveSignal signal, GameObject user, IEnumerable<GameObject> targets)
        {
            if (signal.Equals(GetPassiveSignal()))
                EffectActivation(user, targets);
        }

        void EffectActivation(GameObject user, IEnumerable<GameObject> targets)
        {
            user.GetComponent<DefaultCharacter>().TakeDamage(damage, damageType);
        }
    }

}