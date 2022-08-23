using Character.Character;
using GameManagement;
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

        protected override List<SignalData> EffectAction(CombatSignalData passiveData)
        {
            return passiveData.user.GetComponent<DefaultCharacter>().TakeDamage(damage, damageType);
        }
    }

}