using Character.Character;
using Character.Stats;
using GameManagement;
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

        protected override List<SignalData> EffectAction(CombatSignalData passiveData)
        {
            return passiveData.user.GetComponent<DefaultCharacter>().GainResource(quantity, resourceType);
        }
    }

}
