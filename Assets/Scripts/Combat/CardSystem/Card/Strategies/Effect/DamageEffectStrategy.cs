using CardSystem;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    public abstract class DamageEffectStrategy : EffectStrategy
    {
        public DamageEffectStrategy()
        {
            effectTypes.Add(CardEffectType.Damage);
        }

        public abstract int GetTotalDamage(GameObject user);
    }

}
