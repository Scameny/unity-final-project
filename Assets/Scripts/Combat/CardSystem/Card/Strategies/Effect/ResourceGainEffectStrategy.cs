using CardSystem;
using Character.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    public abstract class ResourceGainEffectStrategy : EffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] protected ResourceType resourceType;
        public ResourceGainEffectStrategy()
        {
            effectTypes.Add(CardEffectType.ResourceGain);
        }

        public ResourceType GetResourceType()
        {
            return resourceType;
        }

        public abstract int GetResourceAmountGained();
    }

}