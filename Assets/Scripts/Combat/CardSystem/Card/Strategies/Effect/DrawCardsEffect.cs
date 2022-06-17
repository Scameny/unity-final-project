using Combat;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    public class DrawCardsEffect : EffectStrategy
    {
        [HideLabel]
        [LabelText("Number of cards")]
        [LabelWidth(120)]
        public int numCards;

        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            foreach (var target in targets)
            {
                TurnCombat characterCombat = target.GetComponent<TurnCombat>();
                characterCombat.DrawCard(numCards);
            }
        }
    }
}
