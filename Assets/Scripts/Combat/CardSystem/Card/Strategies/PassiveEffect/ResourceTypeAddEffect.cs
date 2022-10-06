using Character.Character;
using Character.Stats;
using GameManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{

    public class ResourceTypeAddPassiveEffect : PassiveEffectStrategy
    {
        [SerializeField] ResourceType resourceType;
        [SerializeField] int initialAmount;

        protected override List<SignalData> EffectAction(CombatSignalData passiveData)
        {
            Hero user = passiveData.user.GetComponent<Hero>();
            int maxResource = user.GetClass().GetMaxResourceAmount(user.GetLevel(), resourceType);
            return user.AddResource(resourceType, maxResource, initialAmount);
        }
    }
}