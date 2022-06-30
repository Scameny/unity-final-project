using CardSystem;
using System.Collections;
using System.Collections.Generic;
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
