using System.Collections.Generic;
using Combat;
using UnityEngine;
using Strategies.TargetingStrategies;
using Strategies.FilterStrategies;
using Strategies.EffectStrategies;
using CardSystem;
using NaughtyAttributes;
using UI;

namespace Abilities.ability

{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Ability", order = 1)]
    public class Ability : Usable
    {

        public override CardType GetCardType()
        {
            return CardType.Ability;
        }
    }
}
