using Combat;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [System.Serializable]
    public class DrawCardsEffect : DrawCardsEffectStrategy
    {
        [HideLabel]
        [LabelText("Number of cards")]
        [LabelWidth(120)]
        public int numCards;


        override protected void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            foreach (var target in targets)
            {
                TurnCombat characterCombat = target.GetComponent<TurnCombat>();
                characterCombat.DrawCard(numCards);
            }
        }
    }
}
