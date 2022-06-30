using CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    public abstract class HealEffectStrategy : EffectStrategy
    {
        public HealEffectStrategy()
        {
            effectTypes.Add(CardEffectType.Heal);
        }
    }

}
