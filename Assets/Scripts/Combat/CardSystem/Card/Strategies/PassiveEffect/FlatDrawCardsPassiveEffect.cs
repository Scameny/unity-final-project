using Combat;
using GameManagement;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    public class FlatDrawCardsPassiveEffect : PassiveEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public int cardsDrawed;

        protected override List<SignalData> EffectAction(CombatSignalData passiveData)
        {
            return passiveData.user.GetComponent<TurnCombat>().DrawCard(cardsDrawed);
        }
    }
}
