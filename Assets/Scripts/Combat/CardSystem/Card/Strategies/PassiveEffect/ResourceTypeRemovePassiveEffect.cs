using Character.Character;
using Character.Stats;
using GameManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    public class ResourceTypeRemovePassiveEffect : PassiveEffectStrategy
    {
        [SerializeField] ResourceType resourceType;

        protected override List<SignalData> EffectAction(CombatSignalData passiveData)
        {
            return passiveData.user.GetComponent<DefaultCharacter>().RemoveResource(resourceType);
        }
    }
}