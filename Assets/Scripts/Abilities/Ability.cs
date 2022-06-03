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
    public class Ability : ScriptableObject, IUsable
    {

        [ShowAssetPreview]
        public Sprite sprite;
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;

        public CardType GetCardType()
        {
            return CardType.Ability;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }

        public void Use(GameObject user, IEnumerable<GameObject> targets, Card card)
        {

            user.GetComponent<TurnCombat>().TurnPreparationPause();
            foreach (var filterStrategy in filterStrategies)
            {
                targets = filterStrategy.Filter(targets);
            }
            targetingStrategy.AbilityTargeting(user, targets,
                (IEnumerable<GameObject> targets, bool targetAquired) =>
                {
                    if (targetAquired)
                    {
                        TargetAquired(user, targets);
                        card.CardEffectFinished();
                    }
                    else
                        card.CancelCardUse();
                });

        }

        private void TargetAquired(GameObject user, IEnumerable<GameObject> targets)
        {
            foreach (var effectStrategy in effectStrategies)
            {
                effectStrategy.StartEffect(user, targets);
            }
            user.GetComponent<TurnCombat>().TurnPreparationResume();
        }

    }

    [System.Serializable]
    public class AbilityCard
    {
        public Ability usable;
        public int quantity;
    }
}
