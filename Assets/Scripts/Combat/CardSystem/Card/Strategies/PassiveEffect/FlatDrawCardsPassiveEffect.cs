using Combat;
using GameManagement;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    public class FlatDrawCardsPassiveEffect : PassiveEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public int cardsDrawed;

        protected override void EffectAction(CombatSignalData passiveData)
        {
            passiveData.user.GetComponent<TurnCombat>().DrawCard(cardsDrawed);
        }
    }
}
