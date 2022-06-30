using CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    public abstract class DrawCardsEffectStrategy : EffectStrategy
    {
        public DrawCardsEffectStrategy()
        {
            effectTypes.Add(CardEffectType.DrawCards);
        }
    }
}
