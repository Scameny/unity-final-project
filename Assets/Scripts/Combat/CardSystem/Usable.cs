using Combat;
using NaughtyAttributes;
using Strategies.EffectStrategies;
using Strategies.FilterStrategies;
using Strategies.TargetingStrategies;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public abstract class Usable : ScriptableObject
    {
        [ShowAssetPreview]
        Sprite sprite;

        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
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


        public Sprite GetSprite()
        {
            return sprite;
        }

        abstract public CardType GetCardType();
    }

    [System.Serializable]
    public class UsableCard
    {
        public Usable usable;
        public int quantity;
    }

    public enum CardType
    {
        BasicAttack,
        Ability,
        Item
    }
}
