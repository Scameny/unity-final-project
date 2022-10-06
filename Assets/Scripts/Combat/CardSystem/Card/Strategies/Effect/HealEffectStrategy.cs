using CardSystem;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    public abstract class HealEffectStrategy : EffectStrategy
    {

        public abstract int GetTotalHeal(GameObject user);

        public HealEffectStrategy()
        {
            effectTypes.Add(CardEffectType.Heal);
        }
    }

}
