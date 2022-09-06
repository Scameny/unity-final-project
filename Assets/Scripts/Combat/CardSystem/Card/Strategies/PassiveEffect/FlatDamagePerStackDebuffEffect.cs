using CardSystem;
using Character.Character;
using Character.Buff;
using GameManagement;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    public class FlatDamagePerStackDebuffEffect : PassiveEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] string traitName;
        [SerializeField] int damagePerStack;
        [SerializeField] DamageType damageType;
        [SerializeField] AbilityType abilityType;

        protected override List<SignalData> EffectAction(CombatSignalData passiveData)
        {
            try
            {
                if ((passiveData as CombatCardSignalData).card.GetUsable().GetAbilityType().Equals(abilityType))
                {
                    BuffInfo traitInfo = passiveData.user.GetComponent<DefaultCharacter>().GetTrait(traitName);
                    return passiveData.user.GetComponent<DefaultCharacter>().TakeDamage(traitInfo.stacks * damagePerStack, damageType);
                }
            } 
            catch (NotSpecificTraitException)
            {
            }
            return new List<SignalData>();
        }
    }
}