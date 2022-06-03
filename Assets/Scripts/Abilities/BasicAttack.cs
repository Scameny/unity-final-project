using CardSystem;
using Combat;
using NaughtyAttributes;
using Strategies.EffectStrategies;
using Strategies.FilterStrategies;
using Strategies.TargetingStrategies;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Abilities.BasicAttack
{
    [CreateAssetMenu(fileName = "BasicAttack", menuName = "Abilities/BasicAttack", order = 1)]
    public class BasicAttack : Usable
    {
        override public CardType GetCardType()
        {
            return CardType.BasicAttack;
        }

    }
}
