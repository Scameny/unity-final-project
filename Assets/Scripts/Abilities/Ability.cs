using UnityEngine;
using CardSystem;

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
