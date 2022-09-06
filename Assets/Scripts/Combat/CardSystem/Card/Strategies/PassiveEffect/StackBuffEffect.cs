using CardSystem;
using Character.Character;
using Character.Buff;
using GameManagement;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{ 
    public class StackBuffEffect : PassiveEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public BaseBuff buff;
        [SerializeField] public AbilityType abilityType;
        [SerializeField] public bool noneAbilityTypeCount;

        protected override List<SignalData> EffectAction(CombatSignalData passiveData)
        {
            if ((passiveData as CombatCardSignalData).card.GetUsable().GetAbilityType().Equals(abilityType))
            {
                return passiveData.user.GetComponent<DefaultCharacter>().AddNewTrait(buff);
            }
            else if (!noneAbilityTypeCount && (passiveData as CombatCardSignalData).card.GetUsable().GetAbilityType().Equals(AbilityType.None))
            {
                return new List<SignalData>();
            } 
            else
            {
                return passiveData.user.GetComponent<DefaultCharacter>().RemoveTrait(buff);
            }
        }
    }
}