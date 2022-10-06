using Character.Buff;
using Character.Character;
using GameManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    public class RemoveBuffPassiveEffect : PassiveEffectStrategy
    {
        [SerializeField] string buffName; 
        protected override List<SignalData> EffectAction(CombatSignalData passiveData)
        {
            return passiveData.user.GetComponent<DefaultCharacter>().RemoveTrait(buffName);
        }
    }
}
