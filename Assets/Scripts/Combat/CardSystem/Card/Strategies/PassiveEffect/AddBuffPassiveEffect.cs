using Character.Buff;
using Character.Character;
using GameManagement;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    public class AddBuffPassiveEffect : PassiveEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public BaseBuff buff;

        protected override List<SignalData> EffectAction(CombatSignalData passiveData)
        {
            return passiveData.user.GetComponent<DefaultCharacter>().AddNewTrait(buff);
        }
    }
}