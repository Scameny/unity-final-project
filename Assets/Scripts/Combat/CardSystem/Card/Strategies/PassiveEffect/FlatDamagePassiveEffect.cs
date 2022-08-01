using Character.Character;
using GameManagement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    public class FlatDamagePassiveEffect : PassiveEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public int damage;
        [LabelWidth(120)]
        [SerializeField] public DamageType damageType;

        protected override void EffectAction(CombatSignalData passiveData)
        {
            passiveData.user.GetComponent<DefaultCharacter>().TakeDamage(damage, damageType);
        }
    }

}