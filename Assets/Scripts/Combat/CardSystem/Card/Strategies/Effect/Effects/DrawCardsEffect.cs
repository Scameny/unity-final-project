using Combat;
using GameManagement;
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


        override protected List<SignalData> StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            List<SignalData> signalDatas = new List<SignalData>();
            foreach (var target in targets)
            {
                TurnCombat characterCombat = target.GetComponent<TurnCombat>();
                signalDatas.AddRange(characterCombat.DrawCard(numCards));
            }
            return signalDatas;
        }
    }
}
